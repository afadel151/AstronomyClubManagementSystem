
using Data.Entities.Enums;
namespace Data.Entities.Json;

public class ReducerFlattenerSpecs
{
    public double? ReductionFactor { get; set; }   // 0.79, 0.85 etc — not an enum
    public double? ImageCircleMm { get; set; }     // coverage — important for FF sensors
    public string? ThreadConnection { get; set; }  // "M48", "M54", "T2" — too varied for enum
}