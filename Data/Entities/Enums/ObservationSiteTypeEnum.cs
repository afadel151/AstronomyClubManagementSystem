namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum ObservationSiteTypeEnum
{
    PermanentObservatory,
    DarkSkySite,
    UrbanRooftop,
    IndoorLab,
    Remote,
    Temporary,
    Other
}
