using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;


[Index("MaintenanceDate", Name = "IX_EM_Date", AllDescending = true)]
[Index("EquipmentId", Name = "IX_EM_EquipmentId")]
public partial class EquipmentMaintenance
{
    [Key]
    public int MaintenanceId { get; set; }

    public int EquipmentId { get; set; }

    public DateOnly MaintenanceDate { get; set; }

    [StringLength(30)]
    public EquipmentMaintenanceTypeEnum MaintenanceType { get; set; }

    [StringLength(450)]
    public string PerformedBy { get; set; } = null!;

    [StringLength(2000)]
    public string Description { get; set; } = null!;

    [StringLength(20)]
    public EquipmentMaintenanceResultEnum Result { get; set; }

    public DateOnly? NextDueDate { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? Cost { get; set; }

    [StringLength(500)]
    public string? AttachmentsUrl { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    [ForeignKey("EquipmentId")]
    [InverseProperty("EquipmentMaintenances")]
    public virtual Equipment Equipment { get; set; } = null!;
}
