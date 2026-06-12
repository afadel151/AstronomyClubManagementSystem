

using Domain.Shared.DTO.Equipment.Compatibility;

namespace Domain.Shared.DTO.Equipment.Model;

public sealed record EquipmentModelDetailDto(
    int Id,
    string Code,
    string Name,
    string BrandName,
    string CategoryName,
    bool Accessory,
    string? OpticalDesign,
    string? Specifications,
    string? FitsTelescop,
    string? FitsInstrume,
    int EquipmentsCount,
    DateTimeOffset CreatedAt,
    List<CompatibleWithDto> Compatibles
);

