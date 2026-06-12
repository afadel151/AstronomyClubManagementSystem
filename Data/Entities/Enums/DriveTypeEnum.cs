namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))] 
public enum DriveTypeEnum
{
    Manual,
    Motorized,  // single or dual axis motors, no GoTo
    GoTo        // fully automated with hand controller or software
}