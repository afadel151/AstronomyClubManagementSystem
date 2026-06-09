using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace Web.Club.Providers;

public class ApiHttpClient(IHttpClientFactory factory, NavigationManager nav, ILogger<ApiHttpClient> logger)
{
    private readonly HttpClient _http = factory.CreateClient("Api");
    private readonly NavigationManager _nav = nav;
    private readonly ILogger<ApiHttpClient> _logger = logger;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<T?> GetAsync<T>(string url, CancellationToken ct = default)
    {
        var response = await _http.GetAsync(url, ct);
        return await HandleResponse<T>(response);
    }

    public async Task<T?> PostAsync<T>(string url, object body, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync(url, body, JsonOptions, ct);
        return await HandleResponse<T>(response);
    }

    public async Task<T?> PutAsync<T>(string url, object body, CancellationToken ct = default)
    {
        var response = await _http.PutAsJsonAsync(url, body, JsonOptions, ct);
        return await HandleResponse<T>(response);
    }

    public async Task DeleteAsync(string url, CancellationToken ct = default)
    {
        var response = await _http.DeleteAsync(url, ct);
        await HandleResponse<object>(response);
    }

    public async Task<bool> PostForSuccessAsync(string url, object body, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync(url, body, JsonOptions, ct);
        return await HandleSuccessResponse(response);
    }

    public async Task<bool> PutForSuccessAsync(string url, object body, CancellationToken ct = default)
    {
        var response = await _http.PutAsJsonAsync(url, body, JsonOptions, ct);
        return await HandleSuccessResponse(response);
    }

    public async Task<bool> DeleteForSuccessAsync(string url, CancellationToken ct = default)
    {
        var response = await _http.DeleteAsync(url, ct);
        return await HandleSuccessResponse(response);
    }

    private async Task<T?> HandleResponse<T>(HttpResponseMessage response)
    {
        if (HandleAuthRedirect(response.StatusCode))
        {
            return default;
        }

        if (!response.IsSuccessStatusCode)
        {
            await LogFailedResponseAsync(response);
            response.EnsureSuccessStatusCode();
        }

        if (typeof(T) == typeof(object) || response.StatusCode == HttpStatusCode.NoContent)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    private async Task<bool> HandleSuccessResponse(HttpResponseMessage response)
    {
        if (HandleAuthRedirect(response.StatusCode))
        {
            return false;
        }

        if (!response.IsSuccessStatusCode)
        {
            await LogFailedResponseAsync(response);
        }

        return response.IsSuccessStatusCode;
    }

    private bool HandleAuthRedirect(HttpStatusCode statusCode)
    {
        if (statusCode == HttpStatusCode.Unauthorized)
        {
            _nav.NavigateTo("/account/login", forceLoad: true);
            return true;
        }

        if (statusCode == HttpStatusCode.Forbidden)
        {
            _nav.NavigateTo("/account/access-denied", forceLoad: true);
            return true;
        }

        return false;
    }

    private async Task LogFailedResponseAsync(HttpResponseMessage response)
    {
        var body = response.Content is null
            ? string.Empty
            : await response.Content.ReadAsStringAsync();

        _logger.LogWarning(
            "API request failed with {StatusCode} {ReasonPhrase}. Body: {Body}",
            (int)response.StatusCode,
            response.ReasonPhrase,
            body);
    }
}
