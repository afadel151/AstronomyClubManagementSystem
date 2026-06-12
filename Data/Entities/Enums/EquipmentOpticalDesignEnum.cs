namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]   public enum EquipmentOpticalDesignEnum
{
    Newtonian,
    Sct,
    Refractor,
    RitcheyChretien,
    MaksutovCassegrain,
    MaksutovNewtonian,
    Astrograph,
    Dobsonian,
    Cassegrain
}
