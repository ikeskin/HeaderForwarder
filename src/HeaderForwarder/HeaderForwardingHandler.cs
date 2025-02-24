using Microsoft.AspNetCore.Http;

namespace HeaderForwarder;

public class HeaderForwardingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Dictionary<string, string> _headerMappings;

    public HeaderForwardingHandler(IHttpContextAccessor httpContextAccessor, Dictionary<string, string> headerMappings)
    {
        _httpContextAccessor = httpContextAccessor;
        _headerMappings = headerMappings;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            foreach (var mapping in _headerMappings)
            {
                if (httpContext.Request.Headers.TryGetValue(mapping.Key, out var headerValue))
                {
                    request.Headers.TryAddWithoutValidation(mapping.Value, headerValue.ToString());
                }
            }
        }
        return await base.SendAsync(request, cancellationToken);
    }
}