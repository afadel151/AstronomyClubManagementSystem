
using Data.Entities.Enums;

namespace Domain.Shared.DTO.Equipment.Category;

public record EquipmentCategorylListItemDto(
    int Id,
    string Name,
    string? Description,
    bool Accessory,
    SpecsTypeEnum SpecsType,
    int ModelsCount
);
