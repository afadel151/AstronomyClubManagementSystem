
namespace Domain.Shared.DTO.Equipment.Model;

public record EquipmentModelListItemDto(
    int Id,
    string Name,
    string BrandName,
    string CategoryName,
    bool Accessory,
    int EquipmentsCount,
    DateTimeOffset CreatedAt
);

