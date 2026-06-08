namespace Web.Club.Auth;

public sealed class CircuitTokenStore
{
    public string? AccessToken { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }

    public bool HasValidToken =>
        AccessToken is not null &&
        DateTimeOffset.UtcNow < ExpiresAt.AddMinutes(-2);

    public void Set(string token, DateTimeOffset expiresAt)
    {
        AccessToken = token;
        ExpiresAt   = expiresAt;
    }

    public void Clear()
    {
        AccessToken = null;
        ExpiresAt   = default;
    }
}