using Data.Entities.Enums;

namespace Data.Entities.Json;
public class FilterSpecs
{
    public FilterTypeEnum? FilterType { get; set; }
    
    // Physical size — store in mm, always. Convert for display.
    public double? DiameterMm { get; set; }
    
    // Clip-in is categorically different from round filters
    // A clip-in has no diameter — it fits a specific camera body
    public bool IsClipIn { get; set; } = false;
    
    // Only populated when IsClipIn = true
    public string? ClipInCameraMount { get; set; } // "Canon EF", "Nikon F", "Sony E"
    
    // Bandwidth in nm — useful for narrowband filters
    public double? BandwidthNm { get; set; }
    
    // Central wavelength in nm
    public double? CentralWavelengthNm { get; set; }
}