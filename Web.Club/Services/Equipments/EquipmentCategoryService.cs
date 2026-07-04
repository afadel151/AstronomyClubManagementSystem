using Domain.Shared.DTO;
using Domain.Shared.DTO.Equipment.Category;
using Web.Club.Providers;

namespace Web.Club.Services.Equipments;

public interface IEquipmentCategoryService
{
    Task<PagedResult<EquipmentCategorylListItemDto>?> GetEquipmentCategoryListAsync(PagedQueryParams queryParams);
    Task<EquipmentCategoryDetialsDto?> GetEquipmentCategoryDetialsAsync(int categoryId);
    Task<EquipmentCategorylListItemDto?> CreateCategoryAsync(CreateEquipmentCategoryDto form);
    Task<EquipmentCategorylListItemDto?> UpdateCategoryAsync(int categoryId, UpdateEquipmentCategoryDto form);
    Task<bool> DeleteCategoryAsync(int categoryId);
}

public class EquipmentCategoryService(ApiHttpClient api) : IEquipmentCategoryService
{
    public async Task<PagedResult<EquipmentCategorylListItemDto>?> GetEquipmentCategoryListAsync(
        PagedQueryParams queryParams)
    {
        var orderBy = string.IsNullOrWhiteSpace(queryParams.OrderBy)
            ? "name asc"
            : queryParams.OrderBy;

        var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
        query["PageNumber"] = queryParams.PageNumber.ToString();
        query["PageSize"] = queryParams.PageSize.ToString();
        query["Search"] = queryParams.Search ?? string.Empty;
        query["OrderBy"] = orderBy;

        var url = $"api/equipments/categories?{query}";
        return await api.GetAsync<PagedResult<EquipmentCategorylListItemDto>>(url);
    }

    public async Task<EquipmentCategoryDetialsDto?> GetEquipmentCategoryDetialsAsync(int categoryId)
    {
        var url = $"api/equipments/categories/{categoryId}";
        return await api.GetAsync<EquipmentCategoryDetialsDto>(url);
    }

    public async Task<EquipmentCategorylListItemDto?> CreateCategoryAsync(CreateEquipmentCategoryDto form)
    {
        const string url = "api/equipments/categories";
        return await api.PostAsync<EquipmentCategorylListItemDto>(url, form);
    }

    public async Task<EquipmentCategorylListItemDto?> UpdateCategoryAsync(
        int categoryId,
        UpdateEquipmentCategoryDto form)
    {
        var url = $"api/equipments/categories/{categoryId}";
        return await api.PutAsync<EquipmentCategorylListItemDto>(url, form);
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        var url = $"api/equipments/categories/{categoryId}";
        return await api.DeleteForSuccessAsync(url);
    }
}
