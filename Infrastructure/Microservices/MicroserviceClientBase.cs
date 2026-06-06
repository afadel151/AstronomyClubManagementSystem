using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Microservices;

/// <summary>
/// Base class for typed HTTP clients targeting internal microservices.
/// Provides token forwarding, centralized error mapping, and JSON (de)serialization.
/// </summary>
public abstract class MicroserviceClientBase : IMicroserviceClient
{
    protected readonly HttpClient HttpClient;
    protected readonly ILogger Logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    protected MicroserviceClientBase(
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor,
        IOptions<MicroserviceOptions> options,
        ILogger logger,
        string clientName)
    {
        HttpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        Logger = logger;

        if (options.Value.BaseUrls.TryGetValue(clientName, out var baseUrl))
        {
            HttpClient.BaseAddress = new Uri(baseUrl);
        }
        else
        {
            Logger.LogWarning("Base URL for microservice '{ClientName}' was not found in configuration.", clientName);
        }
        
        HttpClient.Timeout = TimeSpan.FromSeconds(options.Value.DefaultTimeoutSeconds);
    }

    public async Task<TResponse?> GetAsync<TResponse>(string path, CancellationToken ct = default)
    {
        using var request = CreateRequest(HttpMethod.Get, path);
        using var response = await SendRequestAsync(request, ct);
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, ct);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string path, TRequest payload, CancellationToken ct = default)
    {
        using var request = CreateRequest(HttpMethod.Post, path);
        request.Content = JsonContent.Create(payload, options: JsonOptions);
        using var response = await SendRequestAsync(request, ct);
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, ct);
    }

    public async Task PostAsync<TRequest>(string path, TRequest payload, CancellationToken ct = default)
    {
        using var request = CreateRequest(HttpMethod.Post, path);
        request.Content = JsonContent.Create(payload, options: JsonOptions);
        await SendRequestAsync(request, ct);
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string path, TRequest payload, CancellationToken ct = default)
    {
        using var request = CreateRequest(HttpMethod.Put, path);
        request.Content = JsonContent.Create(payload, options: JsonOptions);
        using var response = await SendRequestAsync(request, ct);
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, ct);
    }

    public async Task PutAsync<TRequest>(string path, TRequest payload, CancellationToken ct = default)
    {
        using var request = CreateRequest(HttpMethod.Put, path);
        request.Content = JsonContent.Create(payload, options: JsonOptions);
        await SendRequestAsync(request, ct);
    }

    public async Task DeleteAsync(string path, CancellationToken ct = default)
    {
        using var request = CreateRequest(HttpMethod.Delete, path);
        await SendRequestAsync(request, ct);
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct = default)
    {
        AttachAuthorization(request);
        return await SendRequestAsync(request, ct);
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, string path)
    {
        var request = new HttpRequestMessage(method, path);
        AttachAuthorization(request);
        return request;
    }

    private void AttachAuthorization(HttpRequestMessage request)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        // Forward Authorization header (usually Bearer token)
        var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader["Bearer ".Length..].Trim();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var response = await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);

        if (!response.IsSuccessStatusCode)
        {
            await HandleErrorResponseAsync(response, ct);
        }

        return response;
    }

    private async Task HandleErrorResponseAsync(HttpResponseMessage response, CancellationToken ct)
    {
        var content = await response.Content.ReadAsStringAsync(ct);
        Logger.LogWarning("Microservice HTTP {StatusCode} error from {Uri}: {Content}", 
            response.StatusCode, response.RequestMessage?.RequestUri, content);

        var statusCode = (int)response.StatusCode;
        
        throw statusCode switch
        {
            401 => new UnauthorizedAccessException("Unauthorized request to downstream microservice."),
            // Assuming common exception names exist in Api project, but to avoid circular deps from Infra to Api, 
            // you might want to map these to standard exceptions, or define common exceptions in Domain/Shared.
            // For now, mapping to standard exceptions to avoid adding Api reference to Infra.
            403 => new UnauthorizedAccessException("Forbidden access to downstream microservice."),
            404 => new InvalidOperationException($"Resource not found: {response.RequestMessage?.RequestUri}"),
            409 => new InvalidOperationException($"Conflict from downstream service: {content}"),
            _   => new HttpRequestException($"Downstream service error {(int)response.StatusCode}: {content}", null, response.StatusCode)
        };
    }
}
