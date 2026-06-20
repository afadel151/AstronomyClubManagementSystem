namespace Domain.Shared.Schemas;


public class Country
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string IsoAlpha2 { get; set; } = default!;
}