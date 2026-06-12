namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum TaskStatusEnum
{
    Backlog,
    Todo,
    InProgress,
    Blocked,
    Review,
    Done,
    Cancelled
}
