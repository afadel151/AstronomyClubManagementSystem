
using Domain.Shared.DTO.Equipment.Compatibility;
using Domain.Shared.DTO.Equipment.Upload;

namespace Domain.Shared.DTO.Equipment;
public sealed record CreateEquipmentDto(
    string Code,
    string? SerialNumber,
    int ModelId,
    string Status,
    DateOnly? PurchaseDate,
    decimal? PurchasePriceUs,
    string? Location,
    string? Notes,
    string? FitsTelescop,
    string? FitsInstrume,
    string? Specifications,
    int? ParentEquipmentId
);