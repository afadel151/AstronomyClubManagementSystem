using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Infrastructure.Redis;

/// <summary>
/// Redis-backed session store.
/// Each session lives under key:  {prefix}{sid}
/// Value is a JSON-serialised <see cref="BffSession"/>.
///
/// Concurrency: Redis SET is atomic; no distributed locking is needed here
/// because each session key is only written by one BFF node at a time
/// (the node that owns the incoming request for that sid).
/// </summary>
public sealed class RedisSessionStore : ISessionStore
{
    private readonly IDatabase     _db;
    private readonly RedisOptions  _options;
    private readonly ILogger<RedisSessionStore> _logger;

    private static readonly JsonSerializerOptions _json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public RedisSessionStore(
        IConnectionMultiplexer redis,
        IOptions<RedisOptions>  options,
        ILogger<RedisSessionStore> logger)
    {
        _db      = redis.GetDatabase();
        _options = options.Value;
        _logger  = logger;
    }

    // ── ISessionStore ─────────────────────────────────────────────────────────

    public async Task<string> CreateAsync(BffSession session, CancellationToken ct = default)
    {
        var sid = GenerateSessionId();
        session.CreatedAt  = DateTimeOffset.UtcNow;
        session.LastSeenAt = DateTimeOffset.UtcNow;

        await SetAsync(sid, session);

        _logger.LogInformation(
            "BFF session created: sid={Sid} user={UserId} ip={Ip}",
            sid, session.UserId, session.CreatedByIp);

        return sid;
    }

    public async Task<BffSession?> GetAsync(string sessionId, CancellationToken ct = default)
    {
        var key   = BuildKey(sessionId);
        var value = await _db.StringGetAsync(key);

        if (!value.HasValue)
        {
            _logger.LogDebug("BFF session not found or expired: sid={Sid}", sessionId);
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<BffSession>((string)value!, _json);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialise session: sid={Sid}", sessionId);
            return null;
        }
    }

    public async Task UpdateAsync(string sessionId, BffSession session, CancellationToken ct = default)
    {
        session.LastSeenAt = DateTimeOffset.UtcNow;
        await SetAsync(sessionId, session);

        _logger.LogDebug("BFF session updated (token refresh): sid={Sid}", sessionId);
    }

    public async Task DeleteAsync(string sessionId, CancellationToken ct = default)
    {
        var key     = BuildKey(sessionId);
        var deleted = await _db.KeyDeleteAsync(key);

        _logger.LogInformation(
            "BFF session deleted: sid={Sid} existed={Existed}",
            sessionId, deleted);
    }

    public async Task RefreshExpiryAsync(string sessionId, CancellationToken ct = default)
    {
        var key   = BuildKey(sessionId);
        var expiry = TimeSpan.FromDays(_options.SessionExpiryDays);
        await _db.KeyExpireAsync(key, expiry);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private async Task SetAsync(string sessionId, BffSession session)
    {
        var key    = BuildKey(sessionId);
        var json   = JsonSerializer.Serialize(session, _json);
        var expiry = TimeSpan.FromDays(_options.SessionExpiryDays);

        // SET key value EX seconds — fully atomic
        await _db.StringSetAsync(key, json, expiry);
    }

    private RedisKey BuildKey(string sessionId) =>
        $"{_options.KeyPrefix}{sessionId}";

    /// <summary>
    /// Cryptographically random, URL-safe 32-byte session ID.
    /// Collision probability is negligible for any realistic club size.
    /// </summary>
    private static string GenerateSessionId() =>
        Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32))
               .Replace('+', '-').Replace('/', '_').TrimEnd('=');
}