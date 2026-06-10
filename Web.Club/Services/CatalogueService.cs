using Domain.Shared.DTO;
using Web.Club.Providers;

namespace Web.Club.Services;

public interface ICatalogueService
{
}

public class CatalogueService(ApiHttpClient api) : ICatalogueService
{
    
}