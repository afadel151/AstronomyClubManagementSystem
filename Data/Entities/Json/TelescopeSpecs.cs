using Data.Entities.Enums;
namespace Data.Entities.Json;
public class TelescopeSpecs
{
    public EquipmentOpticalDesignEnum? OpticalDesign { get; set; }
    public int? ApertureMm { get; set; }
    public int? FocalLengthMm { get; set; }
    public double? FocalRatio { get; set; }        
    public double? WeightKg { get; set; }
    public bool? HasFieldFlattener { get; set; }  
}