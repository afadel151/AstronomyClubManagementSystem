namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum MemberStatusEnum
{
    Active,
    Inactive,
    Suspended,
    Alumni,
    Pending
}
