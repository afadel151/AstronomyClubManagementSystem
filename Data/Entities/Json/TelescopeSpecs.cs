// TelescopeSpecs.cs
using Data.Entities.Enums;
namespace Data.Entities.Json;
public class TelescopeSpecs
{
    public EquipmentOpticalDesignEnum? OpticalDesign { get; set; }
    public int? ApertureMm { get; set; }
    public int? FocalLengthMm { get; set; }
    public double? FocalRatio { get; set; }        // computed but worth storing — f/7.5
    public double? WeightKg { get; set; }
    public bool? HasFieldFlattener { get; set; }   // built-in, like some refractors
}