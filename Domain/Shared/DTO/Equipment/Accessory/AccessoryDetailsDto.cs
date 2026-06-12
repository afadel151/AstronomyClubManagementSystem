

using Domain.Shared.DTO.Equipment.Compatibility;
using Domain.Shared.DTO.Equipment.Upload;

namespace Domain.Shared.DTO.Equipment.Accessory;

public sealed record AccessoryDetailDto(
    int Id,
    string Code,
    string? SerialNumber,
    int ModelId,
    string ModelName,
    string BrandName,
    string CategoryName,
    string Status,
    int TotalUsageHours,
    string? Location,
    string? Notes,
    decimal? PurchasePriceUs,
    DateOnly? PurchaseDate,
    DateOnly? RetiredDate,
    string? RetirementReason,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    List<EquipmentUploadDto> Uploads,
    List<CompatibleWithDto> Compatibles
);

