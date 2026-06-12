using Data.Entities.Enums;
namespace Data.Entities.Json;

// FocuserSpecs.cs
public class FocuserSpecs
{
    public FocuserTypeEnum? FocuserType { get; set; }
    public double? TravelMm { get; set; }
    public double? DiameterInch { get; set; }      // 2" or 3" drawtube
    public double? MaxLoadKg { get; set; }
    public bool? IsMotorized { get; set; }
}