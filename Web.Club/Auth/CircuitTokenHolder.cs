namespace Web.Club.Auth;

/// <summary>
/// Holds the JWT access token for the lifetime of a Blazor circuit.
/// Populated once during SSR (when HttpContext has cookies),
/// then reused by BearerTokenHandler during interactive SignalR calls.
/// </summary>
public sealed class CircuitTokenHolder
{
    public string? AccessToken { get; private set; }

    public void Capture(IHttpContextAccessor httpContextAccessor)
    {
        if (AccessToken is not null) return; 

        AccessToken = httpContextAccessor.HttpContext?
            .Request.Cookies[BffAuthenticationDefaults.AccessTokenCookie];
    }
}
