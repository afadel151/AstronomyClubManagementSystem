using Domain.Shared.DTO;
using Domain.Shared.DTO.Equipment.Brand;
using Web.Club.Providers;

namespace Web.Club.Services.Equipments;

public interface IEquipmentBrandService
{
    Task<PagedResult<EquipmentBrandlListItemDto>?> GetEquipmentBrandListAsync(PagedQueryParams queryParams);
    Task<EquipmentBrandDetialsDto?> GetEquipmentBrandDetialsAsync(int BrandId);
    Task<EquipmentBrandlListItemDto?> CreateBrandAsync(CreateEquipmentBrandDto form);
}

public class EquipmentBrandService(ApiHttpClient api) : IEquipmentBrandService
{
    public async Task<PagedResult<EquipmentBrandlListItemDto>?> GetEquipmentBrandListAsync(PagedQueryParams queryParams)
    {
        var orderBy = string.IsNullOrWhiteSpace(queryParams.OrderBy)
           ? "createdAt desc"
           : queryParams.OrderBy;
        var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
        query["PageNumber"] = queryParams.PageNumber.ToString();
        query["PageSize"] = queryParams.PageSize.ToString();
        query["Search"] = queryParams.Search ?? string.Empty;
        query["OrderBy"] = orderBy;

        var url = $"api/equipments/brands?{query}";
        return await api.GetAsync<PagedResult<EquipmentBrandlListItemDto>>(url);
    }

    public async Task<EquipmentBrandDetialsDto?> GetEquipmentBrandDetialsAsync(int BrandId)
    {
        var url = $"api/equipments/brands/{BrandId}";
        return await api.GetAsync<EquipmentBrandDetialsDto>(url);
    }

    public async Task<EquipmentBrandlListItemDto?> CreateBrandAsync(CreateEquipmentBrandDto form)
    {
        var url = "api/equipments/brands";
        return await api.PostAsync<EquipmentBrandlListItemDto>(url,form);
    }
}