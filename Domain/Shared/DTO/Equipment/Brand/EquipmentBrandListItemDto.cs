
namespace Domain.Shared.DTO.Equipment.Brand;

public record EquipmentBrandlListItemDto(
    int Id,
    string Name,
    string Slug,
    string? CountryOfOrigin,
    string? LogoUrl,
    string? Notes,
    int ModelsCount,
    DateTimeOffset CreatedAt
);

