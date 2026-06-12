namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum ProjectMemberRoleEnum
{
    Lead,
    CoLead,
    Contributor,
    Reviewer,
    ObserverOnly,
    Advisor
}
