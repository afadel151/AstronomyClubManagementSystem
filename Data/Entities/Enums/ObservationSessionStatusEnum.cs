namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum ObservationSessionStatusEnum
{
    Planned,
    Ongoing,
    Completed,
    Cancelled,
    Partial
}
