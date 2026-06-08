namespace Infrastructure.Redis;
 
/// <summary>
/// Typed config bound from appsettings "Redis" section.
/// </summary>
public sealed class RedisOptions
{
    public const string SectionName = "Redis";
 
    /// <summary>e.g. "localhost:6379,password=AstroRedis2025,ssl=false"</summary>
    public string ConnectionString { get; set; } = string.Empty;
 
    /// <summary>Key prefix — keeps BFF sessions isolated from any other Redis data.</summary>
    public string KeyPrefix { get; set; } = "astro:session:";
 
    /// <summary>
    /// How long a session lives in Redis.
    /// Should be >= RefreshTokenExpiryDays from JWT options.
    /// </summary>
    public int SessionExpiryDays { get; set; } = 30;
}