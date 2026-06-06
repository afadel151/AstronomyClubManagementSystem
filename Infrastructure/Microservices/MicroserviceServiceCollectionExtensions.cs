using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Microservices;

public static class MicroserviceServiceCollectionExtensions
{
    /// <summary>
    /// Registers a typed microservice client and applies resilient HTTP standard policies.
    /// </summary>
    /// <typeparam name="TClient">The interface type of the client.</typeparam>
    /// <typeparam name="TImplementation">The concrete implementation type deriving from MicroserviceClientBase.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="clientName">The logical name of the client (must match the key in MicroserviceOptions:BaseUrls).</param>
    public static IHttpClientBuilder AddMicroserviceClient<TClient, TImplementation>(
        this IServiceCollection services,
        string clientName)
        where TClient : class, IMicroserviceClient
        where TImplementation : MicroserviceClientBase, TClient
    {
        var builder = services.AddHttpClient<TClient, TImplementation>((provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<MicroserviceOptions>>().Value;
            
            if (options.BaseUrls.TryGetValue(clientName, out var baseUrl))
            {
                client.BaseAddress = new Uri(baseUrl);
            }
        });
        
        builder.AddStandardResilienceHandler();
        
        return builder;
    }
}
