using Application.Repositories;
using Data.Entities.Generated;
using Domain.Shared.DTO;
using Domain.Shared.DTO.Equipment.Model;
using Domain.Shared.DTO.Equipment.Brand;
using Microsoft.EntityFrameworkCore;


namespace Application.Services.Equipments;

public interface IEquipmentModelService
{
}

public sealed class EquipmentModelService(
    IBaseRepository<EquipmentModel> equipmentModelRepository
) : IEquipmentModelService
{
    
}