using Microsoft.AspNetCore.Http;

namespace Web.Club.Auth;

public sealed class BearerTokenHandler(IHttpContextAccessor httpContextAccessor) 
    : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken ct)
    {
        // First try the in-memory store (set after login during the circuit)
        var store = httpContextAccessor.HttpContext?
            .RequestServices.GetService<CircuitTokenStore>();

        var token = store?.HasValidToken == true
            ? store.AccessToken
            : httpContextAccessor.HttpContext?.Request.Cookies["bff_at"];

        if (token is not null)
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        return base.SendAsync(request, ct);
    }
}