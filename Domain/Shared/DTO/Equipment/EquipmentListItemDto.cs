namespace Domain.Shared.DTO.Equipment;
public record EquipmentListItemDto(
    int Id,
    string Code,
    string? SerialNumber,
    string? MainImageKey,
    string ModelName,
    string BrandName,
    string CategoryName,
    string Status,
    int ChildPartsCount,
    string? Location,
    int TotalUsageHours,
    DateTimeOffset CreatedAt
);


public record AccessoryListItemDto(
    int Id,
    string Code,
    string? SerialNumber,
    string? MainImageKey,
    string ModelName,
    string BrandName,
    string CategoryName,
    string Status,
    string? Location,
    int TotalUsageHours,
    DateTimeOffset CreatedAt
);