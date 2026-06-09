using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Domain.Shared.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Web.Club.Auth;

public sealed class BffAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IHttpClientFactory httpClientFactory)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var accessToken = Request.Cookies[BffAuthenticationDefaults.AccessTokenCookie];

        if (!string.IsNullOrWhiteSpace(accessToken)
            && (await TryGetUserAsync(accessToken)) is { } user)
        {
            return AuthenticateResult.Success(CreateTicket(user));
        }

        var refreshToken = Request.Cookies[BffAuthenticationDefaults.RefreshTokenCookie];
        if (string.IsNullOrWhiteSpace(refreshToken))
            return AuthenticateResult.NoResult();

        var refreshed = await TryRefreshAsync(refreshToken);
        if (refreshed is null)
        {
            DeleteAuthCookies();
            return AuthenticateResult.NoResult();
        }

        SetAuthCookies(refreshed.AccessToken, refreshed.RefreshToken, refreshed.ExpiresAt, persistent: true);

        user = await TryGetUserAsync(refreshed.AccessToken);
        return user is null
            ? AuthenticateResult.NoResult()
            : AuthenticateResult.Success(CreateTicket(user));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        var returnUrl = Request.PathBase + Request.Path + Request.QueryString;
        var loginUrl = $"/account/login?returnUrl={Uri.EscapeDataString(returnUrl)}";
        Response.Redirect(loginUrl);
        return Task.CompletedTask;
    }

    protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Response.Redirect("/account/access-denied");
        return Task.CompletedTask;
    }

    private async Task<UserDto?> TryGetUserAsync(string accessToken)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var api = httpClientFactory.CreateClient("AuthApi");
            using var response = await api.SendAsync(request, Context.RequestAborted);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<UserDto>(JsonOptions, Context.RequestAborted);
        }
        catch
        {
            return null;
        }
    }

    private async Task<BffRefreshResponse?> TryRefreshAsync(string refreshToken)
    {
        try
        {
            var api = httpClientFactory.CreateClient("AuthApi");
            using var response = await api.PostAsJsonAsync(
                "/api/auth/refresh",
                new BffRefreshRequest(refreshToken),
                Context.RequestAborted);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<BffRefreshResponse>(
                JsonOptions,
                Context.RequestAborted);
        }
        catch
        {
            return null;
        }
    }

    private AuthenticationTicket CreateTicket(UserDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, GetDisplayName(user)),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new("member_id", user.MemberId)
        };

        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(claims, BffAuthenticationDefaults.AuthenticationScheme);
        return new AuthenticationTicket(
            new ClaimsPrincipal(identity),
            BffAuthenticationDefaults.AuthenticationScheme);
    }

    private void SetAuthCookies(
        string accessToken,
        string refreshToken,
        DateTimeOffset accessTokenExpiresAt,
        bool persistent)
    {
        var secure = Request.IsHttps;

        Response.Cookies.Append(BffAuthenticationDefaults.AccessTokenCookie, accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = secure,
            SameSite = SameSiteMode.Strict,
            Expires = accessTokenExpiresAt,
            Path = "/"
        });

        Response.Cookies.Append(BffAuthenticationDefaults.RefreshTokenCookie, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = secure,
            SameSite = SameSiteMode.Strict,
            Expires = persistent ? DateTimeOffset.UtcNow.AddDays(30) : null,
            Path = "/"
        });
    }

    private void DeleteAuthCookies()
    {
        Response.Cookies.Delete(BffAuthenticationDefaults.AccessTokenCookie);
        Response.Cookies.Delete(BffAuthenticationDefaults.RefreshTokenCookie);
    }

    private static string GetDisplayName(UserDto user)
    {
        var fullName = string.Join(' ', new[] { user.FirstName, user.LastName }
            .Where(part => !string.IsNullOrWhiteSpace(part)));

        return string.IsNullOrWhiteSpace(fullName) ? user.Email : fullName;
    }
}
