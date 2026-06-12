
namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  
public enum SensorTypeEnum
{
    Mono,
    Color,
    OSC        // One-Shot Color — same as Color physically but distinct in astronomy context
}