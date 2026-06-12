using Data.Entities.Enums;
namespace Data.Entities.Json;

// GuiderSpecs.cs
public class GuiderSpecs
{
    public GuiderTypeEnum? GuiderType { get; set; }
    public int? FocalLengthMm { get; set; }        // only for guidescope
    public int? ApertureMm { get; set; }           // only for guidescope
    // OAG has no focal length — it uses the main scope's
}