namespace Infrastructure.Microservices;

public sealed class MicroserviceOptions
{
    public const string SectionName = "Microservices";

    /// <summary>
    /// Base URLs for internal microservices. Key = logical name, Value = base URL.
    /// E.g., { "Library": "http://library-api:8080" }
    /// </summary>
    public Dictionary<string, string> BaseUrls { get; set; } = [];

    /// <summary>Default timeout in seconds for HTTP requests.</summary>
    public int DefaultTimeoutSeconds { get; set; } = 30;

    /// <summary>Number of retries for transient failures (5xx, 408).</summary>
    public int MaxRetries { get; set; } = 3;
}
