using System.Security.Claims;
using Domain.Shared.DTO;
using Microsoft.AspNetCore.Components.Authorization;
using Web.Club.Services;

namespace Web.Club.Auth;

public sealed class ApiCookieAuthenticationStateProvider(
    AuthenticationService authService,
    CircuitTokenStore tokenStore) : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal Anonymous = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {

        var user = await authService.TryRestoreSessionAsync();
        return new AuthenticationState(CreatePrincipal(user));
    }

    public void MarkUserAsAuthenticated(UserDto user) =>
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(CreatePrincipal(user))));

    public void MarkUserAsLoggedOut()
    {
        tokenStore.Clear();
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(Anonymous)));
    }

    private static ClaimsPrincipal CreatePrincipal(UserDto? user)
    {
        if (user is null) return Anonymous;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name,           GetDisplayName(user)),
            new(ClaimTypes.Email,          user.Email),
            new(ClaimTypes.GivenName,      user.FirstName),
            new(ClaimTypes.Surname,        user.LastName),
            new("member_id",               user.MemberId)
        };
        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "Bff"));
    }

    private static string GetDisplayName(UserDto user)
    {
        var full = string.Join(' ',
            new[] { user.FirstName, user.LastName }
                .Where(s => !string.IsNullOrWhiteSpace(s)));
        return string.IsNullOrWhiteSpace(full) ? user.Email : full;
    }
    
}