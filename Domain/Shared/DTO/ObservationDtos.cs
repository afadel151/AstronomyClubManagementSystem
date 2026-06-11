
namespace Domain.Shared.DTO;

public sealed record CreateObservationRequest(
    int       SessionId,
    int       TargetId,
    int       TelescopeId,
    int?      CameraId,
    int?      FilterId,
    int?      MountId,
    int?      GuiderId,
    DateTimeOffset StartTimeUtc,
    DateTimeOffset? EndTimeUtc,
    decimal?  ExposureTimeS,
    string    ObsCollection,
    // ... your other input fields
    string    ObserverId);
 