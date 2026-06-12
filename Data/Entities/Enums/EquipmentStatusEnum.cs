namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum EquipmentStatusEnum
{
    Operational,
    Maintenance,
    Retired,
    Lost,
    Loaned
}
