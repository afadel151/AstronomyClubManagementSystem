namespace Infrastructure.Redis;

/// <summary>
/// The payload stored in Redis under key  astro:session:{sid}.
/// The browser NEVER sees this — it only holds the sid cookie.
/// </summary>
public sealed class BffSession
{
    // ── Token data ────────────────────────────────────────────────────────────
    public string AccessToken  { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset AccessTokenExpiresAt  { get; set; }
    public DateTimeOffset RefreshTokenExpiresAt { get; set; }

    // ── User snapshot (avoids an API call on every request) ───────────────────
    public string UserId    { get; set; } = string.Empty;
    public string Email     { get; set; } = string.Empty;
    public string FullName  { get; set; } = string.Empty;
    public string MemberCode { get; set; } = string.Empty;
    public IList<string> Roles { get; set; } = [];

    // ── Audit / security ──────────────────────────────────────────────────────
    public string?        CreatedByIp  { get; set; }
    public DateTimeOffset CreatedAt    { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastSeenAt   { get; set; } = DateTimeOffset.UtcNow;
}