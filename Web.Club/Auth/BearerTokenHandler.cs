namespace Web.Club.Auth;

public sealed class BearerTokenHandler(IHttpContextAccessor httpContextAccessor) 
    : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken ct)
    {
        var token = httpContextAccessor.HttpContext?
            .Request
            .Cookies[BffAuthenticationDefaults.AccessTokenCookie];

        if (token is not null)
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        return base.SendAsync(request, ct);
    }
}
