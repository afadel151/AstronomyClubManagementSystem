
using Microsoft.AspNetCore.Components.Authorization;
using Web.Club.Providers;
using Web.Club.Auth;
using Web.Club.Services;

namespace Web.Club.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<BffAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp =>
            sp.GetRequiredService<BffAuthenticationStateProvider>());
        services.AddScoped<ApiHttpClient>();
        services.AddScoped<IProfileService,ProfileService>();
        return services;
    }
}
