using Data.Entities.Enums;
using Domain.Shared.DTO.Equipment;
using Domain.Shared.DTO.Equipment.Compatibility;

namespace Domain.Shared.DTO.Equipment.Model;

public sealed record EquipmentModelDetailDto(
    int Id,
    string Name,
    string Slug,
    int BrandId,
    string BrandName,
    int CategoryId,
    string CategoryName,
    bool Accessory,
    SpecsTypeEnum SpecsType,
    string? Url,
    string? Specifications,
    string? FitsTelescop,
    string? FitsInstrume,
    int EquipmentsCount,
    DateTimeOffset CreatedAt,
    List<CompatibleWithDto> Compatibles,
    List<EquipmentListItemDto> Equipments
);
