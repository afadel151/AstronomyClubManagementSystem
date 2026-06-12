using Domain.Shared.DTO.Equipment.Compatibility;
using Domain.Shared.DTO.Equipment.Upload;

namespace Domain.Shared.DTO.Equipment;
public sealed record EquipmentDetailDto(
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
    string? FitsTelescop,
    string? FitsInstrume,
    string? Specifications,
    decimal? PurchasePriceUs,
    DateOnly? PurchaseDate,
    DateOnly? RetiredDate,
    string? RetirementReason,
    int? ParentEquipmentId,
    string? ParentEquipmentCode,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    List<ChildPartDto> ChildParts,
    List<EquipmentUploadDto> Uploads,
    List<CompatibleWithDto> Compatibles
);

public sealed record ChildPartDto(int Id, string Code, string ModelName, string Status);


