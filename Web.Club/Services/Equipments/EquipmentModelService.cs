using Domain.Shared.DTO.Equipment.Model;
using Web.Club.Providers;

namespace Web.Club.Services.Equipments;

public interface IEquipmentModelService
{
    Task<EquipmentModelDetailDto?> GetEquipmentModelDetialsAsync(int modelId);
    Task<EquipmentModelListItemDto?> CreateModelAsync(CreateModelDto form);
    Task<EquipmentModelListItemDto?> UpdateModelAsync(int modelId, UpdateModelDto form);
    Task<bool> DeleteModelAsync(int modelId);
}

public class EquipmentModelService(ApiHttpClient api) : IEquipmentModelService
{
    public async Task<EquipmentModelDetailDto?> GetEquipmentModelDetialsAsync(int modelId)
    {
        var url = $"api/equipments/models/{modelId}";
        return await api.GetAsync<EquipmentModelDetailDto>(url);
    }

    public async Task<EquipmentModelListItemDto?> CreateModelAsync(CreateModelDto form)
    {
        const string url = "api/equipments/models";
        return await api.PostAsync<EquipmentModelListItemDto>(url, form);
    }

    public async Task<EquipmentModelListItemDto?> UpdateModelAsync(int modelId, UpdateModelDto form)
    {
        var url = $"api/equipments/models/{modelId}";
        return await api.PutAsync<EquipmentModelListItemDto>(url, form);
    }

    public async Task<bool> DeleteModelAsync(int modelId)
    {
        var url = $"api/equipments/models/{modelId}";
        return await api.DeleteForSuccessAsync(url);
    }
}
