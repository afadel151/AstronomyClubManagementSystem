namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  
public enum   EquipmentMaintenanceResultEnum
{
    Completed,
    Partial,
    Deferred,
    Failed
}
