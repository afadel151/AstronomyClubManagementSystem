namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum EventGlobalVisibilityEnum
{
    Worldwide,
    NorthernHemisphere,
    SouthernHemisphere,
    Partial,
    Equatorial,
    Polar
}
