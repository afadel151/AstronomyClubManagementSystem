namespace Domain.Shared.DTO.Equipment.Brand;

public sealed record CreateBrandlDto(
    string Name,
    string Slug,
    string? CountryOfOrigin,
    string? LogoUrl,
    string? Notes
);