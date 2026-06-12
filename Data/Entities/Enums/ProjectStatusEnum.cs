namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum ProjectStatusEnum
{
    Draft,
    Active,
    OnHold,
    Completed,
    Archived,
    Cancelled
}
