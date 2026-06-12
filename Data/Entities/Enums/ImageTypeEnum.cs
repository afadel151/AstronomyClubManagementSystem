namespace Data.Entities.Enums;


using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]  public enum ImageTypeEnum
{
    Light,
    Dark,
    Flat,
    Bias,
    MasterDark,
    MasterFlat,
    Stacked,
    Processed,
    Mosaic
}
