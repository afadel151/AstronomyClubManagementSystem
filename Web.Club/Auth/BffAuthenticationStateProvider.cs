using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Web.Club.Auth;

public sealed class BffAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly AuthenticationState Anonymous =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    private readonly ClaimsPrincipal _user;

    public BffAuthenticationStateProvider(
        IHttpContextAccessor httpContextAccessor, 
        CircuitTokenHolder tokenHolder)
    {
        _user = httpContextAccessor.HttpContext?.User ?? Anonymous.User;
        
        tokenHolder.Capture(httpContextAccessor);
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(_user.Identity?.IsAuthenticated == true
            ? new AuthenticationState(_user)
            : Anonymous);
    }
}
