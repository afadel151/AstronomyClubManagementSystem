namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum ProjectPriorityEnum
{
    Low,
    Medium,
    High,
    Critical
}
