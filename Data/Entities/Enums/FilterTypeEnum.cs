
using System.Text.Json.Serialization;

namespace Data.Entities.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FilterTypeEnum
{
    Lum,           // Luminance
    Red,
    Green,
    Blue,
    Ha,            // Hydrogen-Alpha 656nm
    Oiii,          // Oxygen-III 500nm
    Sii,           // Sulfur-II 672nm
    Hb,            // Hydrogen-Beta 486nm
    UvIrCut,       // UV/IR Cut / CLS
    Lpf,           // Light Pollution Filter
    Solar,
    Planetary,
    DualNarrowband, // e.g. Optolong L-eXtreme
    Triband,        // e.g. Optolong L-Ultimate
    Broadband
}