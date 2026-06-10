using Domain.Shared.DTO;
using Web.Club.Providers;

namespace Web.Club.Services;

public interface IEventService
{
}

public class EventService(ApiHttpClient api) : IEventService
{
    
}