namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum ForecastStatusEnum
{
    Proposed,
    Approved,
    Active,
    Achieved,
    Abandoned,
    Deferred
}
