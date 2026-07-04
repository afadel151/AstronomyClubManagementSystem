using Data.Entities.Enums;

namespace Domain.Shared.DTO.Equipment.Category;

public class CreateEquipmentCategoryDto
{
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public bool Accessory { get; set; }
    public SpecsTypeEnum SpecsType { get; set; } = SpecsTypeEnum.None;
}

public class UpdateEquipmentCategoryDto
{
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public bool Accessory { get; set; }
    public SpecsTypeEnum SpecsType { get; set; } = SpecsTypeEnum.None;
}
