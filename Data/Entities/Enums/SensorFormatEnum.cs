using System.Text.Json.Serialization;

namespace Data.Entities.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SensorFormatEnum
{
    FullFrame,        // 36x24mm
    ApsCH,            // 28.7x19mm — Canon APS-H
    ApsC,             // ~23.5x15.6mm
    MicroFourThirds,  // 17.3x13mm
    OneInch,          // 13.2x8.8mm
    FourThirds,       // 17.3x13mm — distinct from M43 mount
    TwoThirdInch,     // 8.8x6.6mm
    HalfInch,         // 6.4x4.8mm
    OneOverOnePointTwo // 9.6x7.2mm — Sony IMX format popular in astro
}