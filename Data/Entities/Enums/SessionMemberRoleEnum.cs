namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum SessionMemberRoleEnum
{
    LeadObserver,
    Observer,
    Imager,
    GuiderOperator,
    NoteTaker,
    Student,
    Guest,
    OutreachPresenter
}
