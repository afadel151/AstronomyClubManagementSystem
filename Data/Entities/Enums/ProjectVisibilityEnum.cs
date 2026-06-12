namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum ProjectVisibilityEnum
{
    Private,
    MembersOnly,
    Public
}
