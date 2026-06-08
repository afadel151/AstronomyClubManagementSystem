namespace Infrastructure.Redis;

/// <summary>
/// Server-side session store for BFF auth sessions.
/// Implementations are backed by Redis (production) or in-memory (testing).
/// </summary>
public interface ISessionStore
{
    /// <summary>Persist a new session. Returns the generated session ID.</summary>
    Task<string> CreateAsync(BffSession session, CancellationToken ct = default);

    /// <summary>Load a session by its ID. Returns null if not found or expired.</summary>
    Task<BffSession?> GetAsync(string sessionId, CancellationToken ct = default);

    /// <summary>Overwrite the token fields of an existing session (silent refresh).</summary>
    Task UpdateAsync(string sessionId, BffSession session, CancellationToken ct = default);

    /// <summary>Delete a session immediately (logout / admin revoke).</summary>
    Task DeleteAsync(string sessionId, CancellationToken ct = default);

    /// <summary>Touch the expiry so sliding sessions stay alive.</summary>
    Task RefreshExpiryAsync(string sessionId, CancellationToken ct = default);
}