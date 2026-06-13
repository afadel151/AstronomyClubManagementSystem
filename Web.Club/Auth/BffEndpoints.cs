using Domain.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using MiniValidation;

namespace Web.Club.Auth;

public static class BffEndpointRouteBuilderExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static IEndpointRouteBuilder MapBffEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/bff").DisableAntiforgery();

        group.MapPost("/login", async (
            [FromForm] LoginFormModel model,
            [FromForm] string? returnUrl,
            [FromServices] IHttpClientFactory factory,
            HttpContext ctx) =>
        {
            if (!MiniValidator.TryValidate(model, out _))
            {
                return Results.Redirect($"/account/login?error=true");
            }

            var api = factory.CreateClient("AuthApi");

            var response = await api.PostAsJsonAsync("/api/auth/login",
                new LoginRequest(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    AuthClientType.Browser));

            if (!response.IsSuccessStatusCode)
                return RedirectWithError("/account/login", returnUrl);

            var result = await Deserialize<LoginResponse>(response);
            if (result is null)
                return RedirectWithError("/account/login", returnUrl);

            SetAuthCookies(ctx, result.AccessToken, result.RefreshToken,
                result.ExpiresAt, model.RememberMe);

            return Results.Redirect(SafeReturnUrl(returnUrl));
        });

        // ── Register ──────────────────────────────────────────────────────────
        group.MapPost("/register", async (
            [FromForm] RegisterFormModel model,
            [FromForm] string? returnUrl,
            [FromServices] IHttpClientFactory factory,
            HttpContext ctx) =>
        {
            if (!MiniValidator.TryValidate(model, out _))
            {
                return Results.Redirect($"/account/register?error=true");
            }

            var api = factory.CreateClient("AuthApi");

            var response = await api.PostAsJsonAsync("/api/auth/register",
                new RegisterRequest
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword,
                    DisplayName = model.DisplayName,
                    Nationality = model.Nationality,
                    City = model.City,
                    AavsoObserverCode = model.AavsoObserverCode,
                    ClientType = AuthClientType.Browser
                });

            if (!response.IsSuccessStatusCode)
                return RedirectWithError("/account/register", returnUrl);

            var result = await Deserialize<RegisterResponse>(response);
            if (result is null)
                return RedirectWithError("/account/register", returnUrl);

            SetAuthCookies(ctx, result.AccessToken, result.RefreshToken,
                result.ExpiresAt, persistent: false);

            return Results.Redirect(SafeReturnUrl(returnUrl));
        });


        group.MapPost("/refresh", async (
            [FromServices] IHttpClientFactory factory,
            HttpContext ctx) =>
        {
            var refreshToken = ctx.Request.Cookies[BffAuthenticationDefaults.RefreshTokenCookie];
            if (refreshToken is null) return Results.Unauthorized();

            var api = factory.CreateClient("AuthApi");
            var response = await api.PostAsJsonAsync("/api/auth/refresh",
                new BffRefreshRequest(refreshToken));

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
            var accessToken = ctx.Request.Cookies[BffAuthenticationDefaults.AccessTokenCookie];
            var refreshToken = ctx.Request.Cookies[BffAuthenticationDefaults.RefreshTokenCookie];

            if (refreshToken is not null)
            {
                try
                {
                    var api = factory.CreateClient("AuthApi");
                    using var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/logout");
                    request.Headers.TryAddWithoutValidation("Cookie", $"refreshToken={refreshToken}");

                    if (!string.IsNullOrWhiteSpace(accessToken))
                    {
                        request.Headers.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                    }

                    await api.SendAsync(request);
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

        ctx.Response.Cookies.Append(BffAuthenticationDefaults.AccessTokenCookie, accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = isHttps,
            SameSite = SameSiteMode.Strict,
            Expires = accessTokenExpiresAt,
            Path = "/"
        });

        // Refresh token — long-lived if persistent
        ctx.Response.Cookies.Append(BffAuthenticationDefaults.RefreshTokenCookie, refreshToken, new CookieOptions
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
        ctx.Response.Cookies.Delete(BffAuthenticationDefaults.AccessTokenCookie);
        ctx.Response.Cookies.Delete(BffAuthenticationDefaults.RefreshTokenCookie);
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
