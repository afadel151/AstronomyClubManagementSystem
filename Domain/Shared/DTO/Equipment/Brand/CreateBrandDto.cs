namespace Domain.Shared.DTO.Equipment.Brand;

public class CreateEquipmentBrandDto
{
    public string Name { get; set; } = "";
    public string Slug { get; set; } = "";
    public string? CountryOfOrigin { get; set; }
    public string? LogoUrl { get; set; }
    public string? Notes { get; set; }
}