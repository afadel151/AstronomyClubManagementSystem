using Application.Repositories;
using Data.Entities.Generated;


namespace Application.Services;

public interface IEquipmentService
{
    Task<List<Equipment>> GetAll();
}

public sealed class EquipmentService(
    IBaseRepository<Equipment> equipmentRepository,
    IBaseRepository<EquipmentBrand> equipmentBrandRepository,
    IBaseRepository<EquipmentModel> equipmentModelRepository,
    IBaseRepository<EquipmentCategory> equipmentCaregoryRepository,
    IBaseRepository<EquipmentUpload> equipmentUploadRepository,
    IBaseRepository<EquipmentMaintenance> equipmentMaintenanceRepository
) : IEquipmentService
{

    public async Task<List<Equipment>> GetAll()
    {
        return await equipmentRepository.GetAllAsync();
    }
}
