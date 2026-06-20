
namespace Domain.Shared.DTO.Equipment.Maintenance;

public sealed record EquipmentMaintenanceDto(
    int MaintenanceId,
    int EquipmentId,
    DateOnly MaintenanceDate,
    string MaintenanceType,
    string PerformedBy,
    string Description,
    string Result,
    DateOnly? NextDueDate,
    decimal? Cost,
    string? AttachmentsUrl,
    DateTimeOffset CreatedAt
);