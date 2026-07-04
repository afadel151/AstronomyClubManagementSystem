using Domain.Shared.DTO.Equipment.Compatibility;

namespace Domain.Shared.DTO.Equipment.Model;

public sealed record CreateModelDto(
    string Name,
    int BrandId,
    int CategoryId,
    string Slug,
    string? Url,
    string? Specifications,
    string? FitsTelescop,
    string? FitsInstrume
);

public sealed record UpdateModelDto(
    string Name,
    int CategoryId,
    string Slug,
    string? Url,
    string? Specifications,
    string? FitsTelescop,
    string? FitsInstrume
);
