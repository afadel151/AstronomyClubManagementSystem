

namespace Data.Entities.Enums;


using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SpecsTypeEnum
{
    None,
    Telescope,
    Mount,
    Camera,
    Filter,
    Guider,
    Focuser,
    ReducerFlattener
}