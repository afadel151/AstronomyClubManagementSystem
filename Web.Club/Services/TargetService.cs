using Domain.Shared.DTO;
using Web.Club.Providers;

namespace Web.Club.Services;

public interface ITargetService
{
}

public class TargetService(ApiHttpClient api) : ITargetService
{
    
}