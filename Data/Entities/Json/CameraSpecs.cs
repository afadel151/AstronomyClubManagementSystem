using Data.Entities.Enums;
namespace Data.Entities.Json;

// CameraSpecs.cs
public class CameraSpecs
{
    public SensorTypeEnum? SensorType { get; set; }
    public SensorFormatEnum? SensorFormat { get; set; }
    public double? PixelSizeUm { get; set; }
    public int? ResolutionWidthPx { get; set; }
    public int? ResolutionHeightPx { get; set; }
    public double? SensorWidthMm { get; set; }     // physical — more useful than format enum for FOV
    public double? SensorHeightMm { get; set; }
    public bool? IsCooled { get; set; }
    public int? FullWellCapacityE { get; set; }
    public double? ReadNoiseE { get; set; }
}