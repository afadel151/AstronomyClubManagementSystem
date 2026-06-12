namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  
public enum MountTypeEnum
{
    // By axis
    AltAzimuth,
    GermanEquatorial,   // most common EQ — worth its own value, distinct enough mechanically
    Fork,               // can be either axis depending on alignment
    Horseshoe,          // large observatory mounts
    Barn,               // barn door tracker
    Panoramic          
}
