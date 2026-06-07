using System.Net.Http.Json;
using Domain.Shared.DTO;
using Web.Club.Auth;

namespace Web.Club.Services;

public sealed class AuthenticationService(HttpClient httpClient)
{
    public async Task<LoginResponse> LoginAsync(string email, string password, bool rememberMe)
    {
        var request = new BrowserLoginRequest(email, password, rememberMe);
        var response = await httpClient.PostAsJsonAsync("/api/auth/login", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return result ?? throw new InvalidOperationException("Failed to parse login response.");
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterFormModel request)
    {
        var browserRequest = new BrowserRegisterRequest(
            request.FullName,
            request.Email,
            request.Password,
            request.ConfirmPassword,
            request.DisplayName,
            request.Nationality,
            request.City,
            request.AavsoObserverCode);

        var response = await httpClient.PostAsJsonAsync("/api/auth/register", browserRequest);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RegisterResponse>();
        return result ?? throw new InvalidOperationException("Failed to parse register response.");
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        try
        {
            var response = await httpClient.GetAsync("/api/auth/me");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || 
                response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return null;
            }
            
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            var response = await httpClient.PostAsync("/api/auth/logout", null);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException)
        {
        }
    }

    private sealed record BrowserLoginRequest(
        string Email,
        string Password,
        bool RememberMe,
        string ClientType = "Browser");

    private sealed record BrowserRegisterRequest(
        string FullName,
        string Email,
        string Password,
        string ConfirmPassword,
        string? DisplayName,
        string? Nationality,
        string? City,
        string? AavsoObserverCode,
        string ClientType = "Browser");
}
