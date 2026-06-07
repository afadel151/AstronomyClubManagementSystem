using Infrastructure.Microservices;
using Infrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Helpers;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();


        services.AddScoped<IStorageService, MinioStorageService>();
       

        return services;
    }
}
