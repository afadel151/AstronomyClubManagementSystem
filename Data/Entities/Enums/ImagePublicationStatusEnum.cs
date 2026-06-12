namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum ImagePublicationStatusEnum
{
    Raw,
    Calibrating,
    Processing,
    Review,
    Approved,
    Published,
    Rejected
}
