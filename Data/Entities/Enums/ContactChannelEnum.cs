namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]
 public enum ContactChannelEnum
{
    Telegram,
    Email,
    Sms,
    Push,
    InApp
}
