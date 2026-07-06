using Domain.Shared.DTO;
using Domain.Shared.DTO.Equipment;
using Domain.Shared.DTO.Equipment.Model;
using Web.Club.Providers;

namespace Web.Club.Services.Equipments;

public interface IEquipmentService
{
    Task<PagedResult<EquipmentListItemDto>?> GetEquipmentListAsync(PagedQueryParams queryParams);
    Task<List<EquipmentModelListItemDto>?> GetModelsList();
    Task<EquipmentListItemDto?> CreateEquipmentAsync(CreateEquipmentDto dto);
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

    public async Task<List<EquipmentModelListItemDto>?> GetModelsList()
    {
        var url = $"api/equipments/models/list";
        return await api.GetAsync<List<EquipmentModelListItemDto>>(url);
    }
    public async Task<EquipmentListItemDto?> CreateEquipmentAsync(CreateEquipmentDto dto)
    {
        var url = "api/equipments";
        return await api.PostAsync<EquipmentListItemDto>(url, dto);
    }
}