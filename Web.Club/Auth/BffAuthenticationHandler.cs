using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Domain.Shared.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Web.Club.Auth;

public sealed class BffAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IHttpClientFactory httpClientFactory,
    IOptions<JwtOptions> jwtOptions)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var accessToken = Request.Cookies[BffAuthenticationDefaults.AccessTokenCookie];

        if (!string.IsNullOrWhiteSpace(accessToken)
            && TryCreateTicket(accessToken) is { } ticket)
        {
            return AuthenticateResult.Success(ticket);
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

        ticket = TryCreateTicket(refreshed.AccessToken);
        return ticket is null
            ? AuthenticateResult.NoResult()
            : AuthenticateResult.Success(ticket);
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

    private AuthenticationTicket? TryCreateTicket(string accessToken)
    {
        try
        {
            var principal = new JwtSecurityTokenHandler
            {
                MapInboundClaims = false
            }.ValidateToken(accessToken, CreateValidationParameters(), out _);

            var identity = new ClaimsIdentity(
                principal.Claims,
                BffAuthenticationDefaults.AuthenticationScheme,
                ClaimTypes.Name,
                ClaimTypes.Role);

            return new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                BffAuthenticationDefaults.AuthenticationScheme);
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

    private TokenValidationParameters CreateValidationParameters() => new()
    {
        ValidateIssuer = true,
        ValidIssuer = _jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = _jwtOptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(2),
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };
}
