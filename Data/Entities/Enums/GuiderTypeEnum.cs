
using System.Text.Json.Serialization;

namespace Data.Entities.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GuiderTypeEnum
{
    Guidescope,    // Dedicated guide scope + guide camera
    Oag,           // Off-Axis Guider
    MultiStar,     // Software multi-star guiding, no separate hardware
    Integrated
}