using Domain.Shared.DTO;
using Web.Club.Providers;

namespace Web.Club.Services;

public interface IForecastService
{
}

public class ForecastService(ApiHttpClient api) : IForecastService
{
    
}