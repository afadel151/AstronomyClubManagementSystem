using Domain.Shared.DTO;
using Domain.Shared.DTO.Equipment;
using Web.Club.Providers;

namespace Web.Club.Services;

public interface IEquipmentService
{
    Task<PagedResult<EquipmentListItemDto>?> GetEquipmentListAsync(PagedQueryParams queryParams);
}

public class EquipmentService(ApiHttpClient api) : IEquipmentService
{
    public async Task<PagedResult<EquipmentListItemDto>?> GetEquipmentListAsync(PagedQueryParams queryParams)
    {
        var orderBy = string.IsNullOrWhiteSpace(queryParams.OrderBy)
           ? "createdAt desc"
           : queryParams.OrderBy;
        var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
        query["PageNumber"] = queryParams.PageNumber.ToString();
        query["PageSize"] = queryParams.PageSize.ToString();
        query["Search"] = queryParams.Search ?? string.Empty;
        query["OrderBy"] = orderBy;

        var url = $"api/equipments?{query}";
        return await api.GetAsync<PagedResult<EquipmentListItemDto>>(url);
    }

}