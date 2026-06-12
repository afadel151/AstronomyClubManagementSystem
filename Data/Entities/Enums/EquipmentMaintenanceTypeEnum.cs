namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]    public enum EquipmentMaintenanceTypeEnum
{
    Collimation,
    Cleaning,
    Repair,
    Calibration,
    FirmwareUpdate,
    Inspection,
    StoragePrep,
    Other
}
