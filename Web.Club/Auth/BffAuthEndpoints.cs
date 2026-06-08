using System.Net.Http.Json;
using System.Security.Claims;
using Infrastructure.Redis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Web.Club.Bff;

// ── DTOs ─────────────────────────────────────────────────────────────────────

/// <summary>What the Blazor page POSTs to /bff/login.</summary>
public sealed record BffLoginRequest(string Email, string Password, bool RememberMe);

/// <summary>What the Auth API returns — mirrors Api.Auth.LoginResponse.</summary>
internal sealed record ApiLoginResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    ApiUserDto User,
    string RefreshToken);   // comes from cookie forwarded by HttpClient, see note below

internal sealed record ApiUserDto(
    string Id,
    string MemberId,
    string Email,
    string FirstName,
    string LastName,
    IList<string> Roles);

// ── Endpoint handler ──────────────────────────────────────────────────────────

/// <summary>
/// Minimal-API handler group for BFF auth endpoints.
/// Registered via MapBffEndpoints() extension in Program.cs.
///
/// Flow:
///   Browser → POST /bff/login (form / JSON)
///           → BFF calls API  POST /api/auth/login
///           → stores tokens in Redis under a generated sid
///           → issues HttpOnly cookie { sid }
///           → Blazor reads HttpContext.User — never a token
/// </summary>
public static class BffAuthEndpoints
{
    public static IEndpointRouteBuilder MapBffEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bff").WithTags("BFF");

        group.MapPost("/login",  LoginAsync) .AllowAnonymous();
        group.MapPost("/logout", LogoutAsync).RequireAuthorization();

        return app;
    }

    // ── POST /bff/login ───────────────────────────────────────────────────────

    private static async Task<IResult> LoginAsync(
        [FromBody]  BffLoginRequest     request,
        [FromServices] IHttpClientFactory  httpFactory,
        [FromServices] ISessionStore        sessionStore,
        HttpContext                         httpContext,
        CancellationToken                   ct)
    {
        // 1. Forward credentials to the Auth API
        var apiClient = httpFactory.CreateClient("AuthApi");

        // We send a plain login payload; the API response contains tokens.
        // NOTE: The Auth API also sets a refreshToken HttpOnly cookie on its response.
        //       We capture it below so we can store it server-side.
        using var apiResponse = await apiClient.PostAsJsonAsync(
            "/api/auth/login",
            new { request.Email, request.Password, request.RememberMe },
            ct);

        if (!apiResponse.IsSuccessStatusCode)
        {
            var detail = await apiResponse.Content.ReadAsStringAsync(ct);
            return Results.Problem(
                detail: detail,
                statusCode: (int)apiResponse.StatusCode,
                title: "Authentication failed");
        }

        var apiResult = await apiResponse.Content.ReadFromJsonAsync<ApiLoginResponse>(ct);
        if (apiResult is null)
            return Results.Problem("Invalid response from auth service.", statusCode: 502);

        // Extract the refresh token from the API's Set-Cookie header so it
        // never touches the browser — we own it server-side from here.
        var refreshToken = ExtractRefreshTokenFromApiResponseInternal(apiResponse)
                           ?? apiResult.RefreshToken; // fallback: body (see AuthController)

        // 2. Build a BffSession and store it in Redis
        var session = new BffSession
        {
            AccessToken             = apiResult.AccessToken,
            RefreshToken            = refreshToken,
            AccessTokenExpiresAt    = apiResult.ExpiresAt,
            // Refresh token expiry comes from options — mirror the API value
            RefreshTokenExpiresAt   = DateTimeOffset.UtcNow.AddDays(30),
            UserId                  = apiResult.User.Id,
            Email                   = apiResult.User.Email,
            FullName                = $"{apiResult.User.FirstName} {apiResult.User.LastName}".Trim(),
            MemberCode              = apiResult.User.MemberId,
            Roles                   = apiResult.User.Roles,
            CreatedByIp             = httpContext.Connection.RemoteIpAddress?.ToString()
        };

        var sid = await sessionStore.CreateAsync(session, ct);

        // 3. Build ClaimsPrincipal from the user snapshot in the session.
        //    Roles are embedded as claims so [Authorize(Roles = "...")] works
        //    without a Redis round-trip on every render.
        var claims = BuildClaims(sid, session);
        var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // 4. Issue the secure session cookie — sid is the only thing in the browser
        var authProps = new AuthenticationProperties
        {
            IsPersistent = request.RememberMe,
            ExpiresUtc   = request.RememberMe
                ? DateTimeOffset.UtcNow.AddDays(30)
                : (DateTimeOffset?)null,
            AllowRefresh = true
        };

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProps);

        // 5. Return minimal user info (no tokens)
        return Results.Ok(new
        {
            apiResult.User.Id,
            apiResult.User.Email,
            apiResult.User.FirstName,
            apiResult.User.LastName,
            apiResult.User.Roles
        });
    }

    // ── POST /bff/logout ──────────────────────────────────────────────────────

    private static async Task<IResult> LogoutAsync(
        [FromServices] ISessionStore sessionStore,
        HttpContext                  httpContext,
        CancellationToken            ct)
    {
        // 1. Revoke the Redis session immediately (instant everywhere)
        var sid = httpContext.User.FindFirstValue("sid");
        if (!string.IsNullOrEmpty(sid))
            await sessionStore.DeleteAsync(sid, ct);

        // 2. Sign out the cookie
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Results.NoContent();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static List<Claim> BuildClaims(string sid, BffSession session)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, session.UserId),
            new(ClaimTypes.Email,          session.Email),
            new(ClaimTypes.Name,           session.FullName),
            new("member_id",               session.MemberCode),
            new("sid",                     sid)                // the pointer to Redis
        };

        claims.AddRange(session.Roles.Select(r => new Claim(ClaimTypes.Role, r)));
        return claims;
    }

    /// <summary>
    /// If the Auth API sets the refresh token as an HttpOnly cookie,
    /// extract it here so it stays server-side.
    /// The cookie name must match AuthConstants.RefreshTokenCookieName ("refreshToken").
    /// </summary>
    internal static string? ExtractRefreshTokenFromApiResponseInternal(HttpResponseMessage response)
    {
        if (!response.Headers.TryGetValues("Set-Cookie", out var cookies))
            return null;

        foreach (var cookie in cookies)
        {
            // Cookie header format: "refreshToken=<value>; HttpOnly; Secure; ..."
            var parts = cookie.Split(';', StringSplitOptions.TrimEntries);
            var tokenPart = parts.FirstOrDefault(p =>
                p.StartsWith("refreshToken=", StringComparison.OrdinalIgnoreCase));

            if (tokenPart is not null)
                return tokenPart["refreshToken=".Length..];
        }

        return null;
    }
}