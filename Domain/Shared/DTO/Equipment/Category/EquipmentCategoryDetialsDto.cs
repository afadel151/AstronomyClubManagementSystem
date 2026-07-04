
using Data.Entities.Enums;
using Domain.Shared.DTO.Equipment.Model;

namespace Domain.Shared.DTO.Equipment.Category;

public record EquipmentCategoryDetialsDto(
    int Id,
    string Name,
    string? Description,
    bool Accessory,
    SpecsTypeEnum SpecsType,
    int ModelsCount,
    int EquipmentsCount,
    List<EquipmentModelListItemDto> Models
);
