using System.Security.Claims;
using Domain.Shared.DTO;
using Microsoft.AspNetCore.Components.Authorization;
using Web.Club.Services;

namespace Web.Club.Auth;

public sealed class ApiCookieAuthenticationStateProvider(AuthenticationService authApiClient)
    : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal Anonymous =
        new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = await authApiClient.GetCurrentUserAsync();
        return new AuthenticationState(CreatePrincipal(user));
    }

    public void MarkUserAsAuthenticated(UserDto user)
    {
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(CreatePrincipal(user))));
    }

    public void MarkUserAsLoggedOut()
    {
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(Anonymous)));
    }

    public async Task RefreshAsync()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        await Task.CompletedTask;
    }

    private static ClaimsPrincipal CreatePrincipal(UserDto? user)
    {
        if (user is null)
        {
            return Anonymous;
        }

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

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "ApiCookie"));
    }

    private static string GetDisplayName(UserDto user)
    {
        var fullName = string.Join(' ', new[] { user.FirstName, user.LastName }
            .Where(value => !string.IsNullOrWhiteSpace(value)));

        return string.IsNullOrWhiteSpace(fullName) ? user.Email : fullName;
    }
}
