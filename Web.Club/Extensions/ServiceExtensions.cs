
using Microsoft.AspNetCore.Components.Authorization;
using Web.Club.Providers;
using Web.Club.Auth;
using Web.Club.Services;
using Web.Club.Services.Equipments;

namespace Web.Club.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IEquipmentService,EquipmentService>();
        services.AddScoped<BffAuthenticationStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp =>
            sp.GetRequiredService<BffAuthenticationStateProvider>());
        services.AddScoped<ApiHttpClient>();
        services.AddScoped<IProfileService,ProfileService>();
        services.AddScoped<ICatalogueService,CatalogueService>();
        services.AddScoped<IChatService,ChatService>();
        services.AddScoped<IEventService,EventService>();
        services.AddScoped<IForecastService,ForecastService>();
        services.AddScoped<IMemberService,MemberService>();
        services.AddScoped<IObservationService,ObservationService>();
        services.AddScoped<IProjectService,ProjectService>();
        services.AddScoped<ITargetService,TargetService>();
        services.AddScoped<IEquipmentBrandService,EquipmentBrandService>();
        services.AddScoped<IEquipmentCategoryService,EquipmentCategoryService>();
        services.AddScoped<IEquipmentModelService,EquipmentModelService>();
        return services;
    }
}
