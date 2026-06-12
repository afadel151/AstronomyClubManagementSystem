namespace Domain.Shared.DTO.Equipment.Accessory;

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