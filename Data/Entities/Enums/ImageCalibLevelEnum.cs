namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum ImageCalibLevelEnum : byte
{
    Raw,
    Calibrated,
    ScienceReady,
    Derived
}
