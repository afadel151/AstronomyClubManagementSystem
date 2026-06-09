namespace Web.Club.Http;

/// <summary>
/// Forwards the browser's incoming cookies to the upstream API request,
/// and pipes any Set-Cookie headers from the API response back to the browser.
/// This is the critical BFF glue: without it, cookies live only in the server's
/// memory and the browser never sees them.
/// </summary>
public sealed class CookieForwardingHandler(IHttpContextAccessor httpContextAccessor)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var ctx = httpContextAccessor.HttpContext;

        if (ctx is not null)
        {
            // Forward all cookies the browser sent to us → upstream API
            var cookieHeader = ctx.Request.Headers.Cookie.ToString();
            if (!string.IsNullOrEmpty(cookieHeader))
            {
                request.Headers.Remove("Cookie");
                request.Headers.TryAddWithoutValidation("Cookie", cookieHeader);
            }
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (ctx is not null)
        {
            // Pipe any Set-Cookie the API set → back to the browser response
            if (response.Headers.TryGetValues("Set-Cookie", out var setCookies))
            {
                foreach (var cookie in setCookies)
                {
                    ctx.Response.Headers.Append("Set-Cookie", cookie);
                }
            }
        }

        return response;
    }
}