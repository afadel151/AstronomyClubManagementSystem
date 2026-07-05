
using Data.Entities.Enums;

namespace Domain.Shared.DTO.Equipment;
public sealed record CreateEquipmentDto(
    int ModelId,
    EquipmentStatusEnum Status,
    string? SerialNumber,
    DateOnly? PurchaseDate,
    decimal? PurchasePriceUs,
    string? Location,
    string? Notes,
    string? FitsTelescop,
    string? FitsInstrume
);