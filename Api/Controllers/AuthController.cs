using Application.Auth;
using Application.Services;
using Domain.Shared.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    IAuthService authService,
    IOptions<AuthOptions> authOptions) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken ct)
    {
        var response = await authService.LoginAsync(request, GetIpAddress(), ct);
        SetRefreshTokenCookie(response.RefreshToken, request.RememberMe);

        return Ok(response);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request, CancellationToken ct)
    {
        var response = await authService.RegisterAsync(request, GetIpAddress(), ct);
        SetRefreshTokenCookie(response.RefreshToken, rememberMe: false);

        return Ok(response);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<RefreshResponse>> Refresh(CancellationToken ct)
    {
        var response = await authService.RefreshAsync(GetRefreshTokenCookie(), GetIpAddress(), ct);
        SetRefreshTokenCookie(response.RefreshToken, rememberMe: true);

        return Ok(response);
    }

    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        await authService.LogoutAsync(GetRefreshTokenCookie(), GetIpAddress(), ct);
        Response.Cookies.Delete(AuthConstants.RefreshTokenCookieName);

        return NoContent();
    }

    [HttpGet("me")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<UserDto> Me()
    {
        // Build UserDto entirely from claims — no DB hit.
        return Ok(authService.GetCurrentUserFromClaims(User));
    }

    private string? GetRefreshTokenCookie() =>
        Request.Cookies.TryGetValue(AuthConstants.RefreshTokenCookieName, out var token) ? token : null;

    private string? GetIpAddress() =>
        HttpContext.Connection.RemoteIpAddress?.ToString();

    private void SetRefreshTokenCookie(string refreshToken, bool rememberMe)
    {
        Response.Cookies.Append(
            AuthConstants.RefreshTokenCookieName,
            refreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = rememberMe ? DateTimeOffset.UtcNow.AddDays(authOptions.Value.RefreshTokenExpiryDays) : null
            });
    }
}
