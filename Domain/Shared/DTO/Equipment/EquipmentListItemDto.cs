using Data.Entities.Enums;

namespace Domain.Shared.DTO.Equipment;
public record EquipmentListItemDto(
    int Id,
    string Code,
    string? SerialNumber,
    string? MainImagePresignedUrl,
    string? MainImageKey,
    string ModelName,
    string BrandName,
    string CategoryName,
    EquipmentStatusEnum Status,
    int ChildPartsCount,
    string? Location,
    int TotalUsageHours,
    DateTimeOffset CreatedAt
);


