namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum TaskPriorityEnum
{
    Low,
    Medium,
    High,
    Critical
}
