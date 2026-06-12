// MountSpecs.cs

using Data.Entities.Enums;
namespace Data.Entities.Json;
public class MountSpecs
{
    public MountTypeEnum? MountType { get; set; }
    public TrackingAxisEnum? TrackingAxis { get; set; }
    public DriveTypeEnum? DriveType { get; set; }
    public double? MaxPayloadKg { get; set; }
    public bool? HasPolarScope { get; set; }
    public bool? HasGuidePort { get; set; }        // ST-4 port — relevant for guiding setup
    public double? WeightKg { get; set; }
}