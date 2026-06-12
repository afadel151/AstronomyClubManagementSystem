
namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))] 
public enum FocuserTypeEnum
{
    RackAndPinion,
    Crayford,
    LinearBearing,  // Moonlite, Pegasus style — Crayford variant but meaningfully different
                    // in load capacity and slip resistance
    Stepper
}