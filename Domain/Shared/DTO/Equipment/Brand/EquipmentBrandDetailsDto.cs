
using Domain.Shared.DTO.Equipment.Model;

namespace Domain.Shared.DTO.Equipment.Brand;

public record EquipmentBrandDetialsDto(
    int Id,
    string Name,
    string Slug,
    string? CountryOfOrigin,
    string? LogoUrl,
    string? Notes,
    int ModelsCount,
    List<EquipmentModelListItemDto> Models,
    DateTimeOffset CreatedAt
);

