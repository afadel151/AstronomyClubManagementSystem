using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Web.Club.Bff;

/// <summary>
/// Blazor Server AuthenticationStateProvider backed by HttpContext.
///
/// Key rules (from the spec):
///   ✔ Reads from HttpContext.User — the server is the single source of truth.
///   ✔ Does NOT call /me or any API.
///   ✔ Does NOT reconstruct the ClaimsPrincipal manually.
///   ✔ Triggers UI re-render when the session cookie changes (e.g. after logout).
///
/// Why this is correct for Blazor Server:
///   In Blazor Server every circuit is associated with a single HTTP connection.
///   The HttpContext that created the circuit carries the validated cookie and the
///   populated ClaimsPrincipal. We capture it once and expose it to the component
///   tree via CascadingAuthenticationState.
/// </summary>
public sealed class ServerAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly AuthenticationState Anonymous =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    private readonly IHttpContextAccessor _accessor;

    public ServerAuthenticationStateProvider(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = _accessor.HttpContext?.User;

        // If there is no HttpContext (pre-render / background) return anonymous
        if (user?.Identity?.IsAuthenticated != true)
            return Task.FromResult(Anonymous);

        return Task.FromResult(new AuthenticationState(user));
    }

    /// <summary>
    /// Call this after a BFF login or logout completes so that
    /// CascadingAuthenticationState updates all subscribed components.
    /// </summary>
    public void NotifyAuthStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}