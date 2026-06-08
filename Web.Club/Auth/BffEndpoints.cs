using Domain.Shared.DTO;
using Domain.Shared.Schemas;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;

namespace Web.Club.Auth;

public static class BffEndpointRouteBuilderExtensions
{
    private const string AccessTokenCookie = "bff_at";
    private const string RefreshTokenCookie = "bff_rt";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static IEndpointRouteBuilder MapBffEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/bff").DisableAntiforgery();

        group.MapPost("/login", async (
            [FromForm] string email,
            [FromForm] string password,
            [FromForm] bool rememberMe,
            [FromForm] string? returnUrl,
            [FromServices] IHttpClientFactory factory,
            HttpContext ctx) =>
        {
            var api = factory.CreateClient("Api");
            var response = await api.PostAsJsonAsync("/api/auth/login",
                new LoginRequest(email, password, rememberMe, AuthClientType.Browser));

            if (!response.IsSuccessStatusCode)
                return RedirectWithError("/account/login", returnUrl);

            var result = await Deserialize<LoginResponse>(response);
            if (result is null) return RedirectWithError("/account/login", returnUrl);

            SetAuthCookies(ctx, result.AccessToken, result.RefreshToken,
                result.ExpiresAt, rememberMe);

            return Results.Redirect(SafeReturnUrl(returnUrl));
        });

        // ── Register ──────────────────────────────────────────────────────────
        group.MapPost("/register", async (
            [FromForm] string fullName,
            [FromForm] string email,
            [FromForm] string password,
            [FromForm] string confirmPassword,
            [FromForm] string? displayName,
            [FromForm] string? nationality,
            [FromForm] string? city,
            [FromForm] string? aavsoObserverCode,
            [FromForm] string? returnUrl,
            [FromServices] IHttpClientFactory factory,
            HttpContext ctx) =>
        {
            var api = factory.CreateClient("Api");
            var response = await api.PostAsJsonAsync("/api/auth/register",
                new RegisterRequest
                {
                    FullName = fullName,
                    Email = email,
                    Password = password,
                    ConfirmPassword = confirmPassword,
                    DisplayName = displayName,
                    Nationality = nationality,
                    City = city,
                    AavsoObserverCode = aavsoObserverCode,
                    ClientType = AuthClientType.Browser
                });

            if (!response.IsSuccessStatusCode)
                return RedirectWithError("/account/register", returnUrl);

            var result = await Deserialize<RegisterResponse>(response);
            if (result is null) return RedirectWithError("/account/register", returnUrl);

            SetAuthCookies(ctx, result.AccessToken, result.RefreshToken,
                result.ExpiresAt, persistent: false);

            return Results.Redirect(SafeReturnUrl(returnUrl));
        });

        // ── Refresh ───────────────────────────────────────────────────────────
        // Called by TryRestoreSessionAsync at circuit start.
        // Reads bff_rt, exchanges it with the API, rotates both cookies.
        group.MapPost("/refresh", async (
            [FromServices] IHttpClientFactory factory,
            HttpContext ctx) =>
        {
            var rt = ctx.Request.Cookies[RefreshTokenCookie];
            if (rt is null) return Results.Unauthorized();

            var api = factory.CreateClient("Api");
            var response = await api.PostAsJsonAsync("/api/auth/refresh/bff",
                new { RefreshToken = rt });

            if (!response.IsSuccessStatusCode)
            {
                DeleteAuthCookies(ctx);
                return Results.Unauthorized();
            }

            var result = await Deserialize<BffRefreshResponse>(response);
            if (result is null)
            {
                DeleteAuthCookies(ctx);
                return Results.Unauthorized();
            }

            SetAuthCookies(ctx, result.AccessToken, result.RefreshToken,
                result.ExpiresAt, persistent: true);

            return Results.Ok(new { result.AccessToken, result.ExpiresAt });
        });

        // ── Logout ────────────────────────────────────────────────────────────
        group.MapGet("/logout", async (
            [FromServices] IHttpClientFactory factory,
            HttpContext ctx) =>
        {
            var rt = ctx.Request.Cookies[RefreshTokenCookie];
            if (rt is not null)
            {
                try
                {
                    var api = factory.CreateClient("Api");
                    await api.PostAsJsonAsync("/api/auth/logout",
                        new { RefreshToken = rt });
                }
                catch { }
            }

            DeleteAuthCookies(ctx);
            return Results.Redirect("/account/login");
        });

        return endpoints;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static void SetAuthCookies(
        HttpContext ctx,
        string accessToken,
        string refreshToken,
        DateTimeOffset accessTokenExpiresAt,
        bool persistent)
    {
        var isHttps = ctx.Request.IsHttps;

        // Access token — expires with the token itself
        ctx.Response.Cookies.Append(AccessTokenCookie, accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = isHttps,
            SameSite = SameSiteMode.Strict,
            Expires = accessTokenExpiresAt,
            Path = "/"
        });

        // Refresh token — long-lived if persistent
        ctx.Response.Cookies.Append(RefreshTokenCookie, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = isHttps,
            SameSite = SameSiteMode.Strict,
            Expires = persistent ? DateTimeOffset.UtcNow.AddDays(30) : null,
            Path = "/"
        });
    }

    private static void DeleteAuthCookies(HttpContext ctx)
    {
        ctx.Response.Cookies.Delete(AccessTokenCookie);
        ctx.Response.Cookies.Delete(RefreshTokenCookie);
    }

    private static IResult RedirectWithError(string path, string? returnUrl)
    {
        var safe = string.IsNullOrWhiteSpace(returnUrl)
            ? string.Empty
            : $"&returnUrl={Uri.EscapeDataString(returnUrl)}";
        return Results.Redirect($"{path}?error=true{safe}");
    }

    private static string SafeReturnUrl(string? returnUrl, string fallback = "/")
    {
        if (string.IsNullOrWhiteSpace(returnUrl)) return fallback;

        // Prevent open redirect
        return returnUrl.StartsWith('/') && !returnUrl.StartsWith("//")
            ? returnUrl
            : fallback;
    }

    private static async Task<T?> Deserialize<T>(HttpResponseMessage response)
    {
        var raw = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(raw, JsonOptions);
    }
}