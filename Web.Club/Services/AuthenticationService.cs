using System.Net.Http.Json;
using System.Text.Json;
using Domain.Shared.DTO;
using Microsoft.AspNetCore.Http;
using Web.Club.Auth;

namespace Web.Club.Services;

public sealed class AuthenticationService(
    IHttpClientFactory httpClientFactory,
    IHttpContextAccessor httpContextAccessor,
    CircuitTokenStore tokenStore)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // Calls the upstream API — BearerTokenHandler injects the JWT
    private HttpClient Api => httpClientFactory.CreateClient("Api");

    // Calls the Blazor server's own BFF endpoints
    private HttpClient Bff => httpClientFactory.CreateClient("Bff");

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        try
        {
            var response = await Api.GetAsync("/api/auth/me");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<UserDto>(JsonOptions);
        }
        catch { return null; }
    }

    public async Task LogoutAsync()
    {
        try { await Bff.PostAsync("/bff/logout", null); }
        catch { }
        finally { tokenStore.Clear(); }
    }

    // Called once at circuit start by the AuthenticationStateProvider.
    // Tries to restore the session using the bff_rt cookie if the
    // circuit token store is empty (e.g. after a page reload).
    public async Task<UserDto?> TryRestoreSessionAsync()
    {
        // Circuit already has a valid in-memory token — attach and fetch user
        if (tokenStore.HasValidToken)
            return await GetCurrentUserAsync();

        // Check if the access token cookie is still alive (not yet expired)
        // BearerTokenHandler will attach it automatically for the /me call
        var at = httpContextAccessor.HttpContext?.Request.Cookies["bff_at"];
        if (at is not null)
            return await GetCurrentUserAsync();

        // Access token cookie expired — try to refresh using the refresh cookie
        var rt = httpContextAccessor.HttpContext?.Request.Cookies["bff_rt"];
        if (rt is null) return null;

        try
        {
            // POST to the BFF refresh endpoint — it reads bff_rt itself,
            // calls the API, rotates both cookies, returns the new access token
            var response = await Bff.PostAsync("/bff/refresh", null);
            if (!response.IsSuccessStatusCode) return null;

            var result = await Deserialize<BffRefreshResult>(response);
            if (result is null) return null;

            // Populate the circuit store so subsequent API calls in this
            // circuit use the in-memory token instead of the cookie
            tokenStore.Set(result.AccessToken, result.ExpiresAt);
            return await GetCurrentUserAsync();
        }
        catch { return null; }
    }

    private static async Task<T?> Deserialize<T>(HttpResponseMessage response)
    {
        var raw = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(raw, JsonOptions);
    }

    private sealed record BffRefreshResult(
        string AccessToken,
        DateTimeOffset ExpiresAt);
}