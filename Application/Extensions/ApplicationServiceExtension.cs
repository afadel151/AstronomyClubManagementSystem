using Application.Factories;
using Application.Repositories;
using Application.Services;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IForecastService, ForecastService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<ITargetService, TargetService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IObservationService, ObservationService>();
        services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

        return services;
    }
}
