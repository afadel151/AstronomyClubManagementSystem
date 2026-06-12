namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum NotificationStatusEnum
{
    Pending,
    Sending,
    Sent,
    Failed,
    Skipped,
    Cancelled
}
