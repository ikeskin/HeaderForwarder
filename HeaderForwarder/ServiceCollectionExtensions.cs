using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace HeaderForwarder;

public static class ServiceCollectionExtensions
{
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddHeaderForwarding(this IHttpClientBuilder builder, Dictionary<string, string> headerMappings)
        {
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.AddHttpMessageHandler(sp => new HeaderForwardingHandler(sp.GetRequiredService<IHttpContextAccessor>(), headerMappings));
            return builder;
        }
    }
}
