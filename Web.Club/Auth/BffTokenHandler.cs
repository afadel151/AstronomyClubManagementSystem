using System.Net;
using System.Net.Http.Headers;
using Infrastructure.Redis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;

namespace Web.Club.Bff;

/// <summary>
/// DelegatingHandler injected into every named/typed HttpClient that calls the Auth API.
///
/// On every outbound request:
///   1. Reads sid claim from HttpContext.User
///   2. Loads BffSession from Redis
///   3. Silently refreshes the access token if it is close to expiry
///   4. Attaches Authorization: Bearer {access_token}
///
/// The browser never sees a token. It only ever sends the session cookie.
/// </summary>
public sealed class BffTokenHandler(
    IHttpContextAccessor                    httpContextAccessor,
    ISessionStore                           sessionStore,
    IHttpClientFactory                      httpClientFactory,
    ILogger<BffTokenHandler>                logger) : DelegatingHandler
{
    // Refresh the access token if it expires within this window.
    private static readonly TimeSpan RefreshBuffer = TimeSpan.FromMinutes(2);

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage  request,
        CancellationToken   ct)
    {
        var ctx = httpContextAccessor.HttpContext;
        if (ctx is null)
            return await base.SendAsync(request, ct); // non-HTTP context (background job)

        var sid = ctx.User.FindFirst("sid")?.Value;
        if (string.IsNullOrEmpty(sid))
        {
            logger.LogWarning("BffTokenHandler: no sid claim — anonymous request.");
            return await base.SendAsync(request, ct);
        }

        // 1. Load session from Redis
        var session = await sessionStore.GetAsync(sid, ct);
        if (session is null)
        {
            // Session was revoked (logout from another device, admin action, etc.)
            // Force a cookie sign-out so the browser is redirected to login.
            await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                ReasonPhrase = "Session revoked"
            };
        }

        // 2. Silent refresh if the access token is expiring soon
        if (session.AccessTokenExpiresAt - DateTimeOffset.UtcNow < RefreshBuffer)
        {
            logger.LogInformation("BffTokenHandler: access token near expiry — refreshing. sid={Sid}", sid);
            session = await RefreshTokensAsync(sid, session, ct);

            if (session is null)
            {
                // Refresh failed (refresh token expired/revoked) — force logout
                await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await sessionStore.DeleteAsync(sid, ct);
                return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    ReasonPhrase = "Refresh token expired"
                };
            }
        }

        // 3. Attach the access token — browser never sees this
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", session.AccessToken);

        // 4. Touch the Redis expiry (sliding session)
        await sessionStore.RefreshExpiryAsync(sid, ct);

        return await base.SendAsync(request, ct);
    }

    // ── Silent refresh ────────────────────────────────────────────────────────

    /// <summary>
    /// Calls POST /api/auth/refresh with the server-side refresh token,
    /// updates Redis, returns the updated session or null on failure.
    /// </summary>
    private async Task<BffSession?> RefreshTokensAsync(
        string     sid,
        BffSession session,
        CancellationToken ct)
    {
        try
        {
            // Use a bare HttpClient (not the one being intercepted) to avoid recursion
            var authClient = httpClientFactory.CreateClient("AuthApiDirect");

            // The Auth API expects the refresh token as an HttpOnly cookie.
            // We inject it manually from our server-side store.
            var refreshRequest = new HttpRequestMessage(HttpMethod.Post, "/api/auth/refresh");
            refreshRequest.Headers.Add("Cookie",
                $"refreshToken={session.RefreshToken}");

            using var response = await authClient.SendAsync(refreshRequest, ct);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning(
                    "BffTokenHandler: refresh call failed {Status}. sid={Sid}",
                    response.StatusCode, sid);
                return null;
            }

            var refreshed = await response.Content
                .ReadFromJsonAsync<RefreshApiResponse>(ct);

            if (refreshed is null)
                return null;

            // Extract new refresh token from the API cookie header
            var newRefreshToken = BffAuthEndpoints.ExtractRefreshTokenFromApiResponseInternal(response)
                                  ?? session.RefreshToken; // keep old if API didn't rotate it

            // Update the Redis session in place
            session.AccessToken          = refreshed.AccessToken;
            session.AccessTokenExpiresAt = refreshed.ExpiresAt;
            session.RefreshToken         = newRefreshToken;

            await sessionStore.UpdateAsync(sid, session, ct);

            logger.LogInformation("BffTokenHandler: token refreshed silently. sid={Sid}", sid);
            return session;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "BffTokenHandler: exception during token refresh. sid={Sid}", sid);
            return null;
        }
    }

    private sealed record RefreshApiResponse(string AccessToken, DateTimeOffset ExpiresAt);
}