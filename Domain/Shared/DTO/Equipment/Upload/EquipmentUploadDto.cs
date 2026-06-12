
namespace Domain.Shared.DTO.Equipment.Upload;
public sealed record EquipmentUploadDto(int Id, string ObjectKey, string? Caption, DateTimeOffset CreatedAt);
