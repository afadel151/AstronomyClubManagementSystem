using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Microservices;

// An example implementation for demonstration
public interface IExampleServiceClient : IMicroserviceClient
{
    Task<string?> GetServerStatusAsync(CancellationToken ct = default);
}

public sealed class ExampleServiceClient(
    HttpClient httpClient,
    IHttpContextAccessor httpContextAccessor,
    IOptions<MicroserviceOptions> options,
    ILogger<ExampleServiceClient> logger)
    : MicroserviceClientBase(httpClient, httpContextAccessor, options, logger, "Example")
    , IExampleServiceClient
{
    public async Task<string?> GetServerStatusAsync(CancellationToken ct = default)
    {
        return await GetAsync<string>("/api/status", ct);
    }
}
