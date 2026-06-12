using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;

[Index("Status", Name = "IX_EQ_Status")]
[Index("Code", Name = "UK_EQUIPMENTS_Code", IsUnique = true)]
public partial class Equipment
{
    // identity
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? SerialNumber { get; set; }
    public int ModelId { get; set; }
    
    // status
    public EquipmentStatusEnum Status { get; set; }



    // purchase
    public DateOnly? PurchaseDate { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? PurchasePriceUs { get; set; }

    // location
    [StringLength(200)]
    public string? Location { get; set; }
    

    [StringLength(1000)]
    public string? Notes { get; set; }

    // hierarchy
    public int? ParentEquipmentId { get; set; }  

    [ForeignKey("ParentEquipmentId")]
    public virtual Equipment? ParentEquipment { get; set; }

    [InverseProperty("ParentEquipment")]
    public virtual ICollection<Equipment> ChildParts { get; set; } = [];

    // usage
    public int TotalUsageHours {get;set;}

    // retirement
    public DateOnly? RetiredDate { get; set; }
    [StringLength(500)]
    public string? RetirementReason { get; set; }

    // model
    [ForeignKey("ModelId")]
    [InverseProperty("Equipments")]
    public virtual EquipmentModel EquipmentModel  { get; set; } = null!;

    // timestamps
   public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    [InverseProperty("Equipment")]
    public virtual ICollection<EquipmentMaintenance> EquipmentMaintenances { get; set; } = [];

    [InverseProperty("Equipment")]
    public virtual ICollection<EquipmentUpload> EquipmentUploads { get; set; } = [];

    [InverseProperty("Camera")]
    public virtual ICollection<Observation> ObservationCameras { get; set; } = [];

    [InverseProperty("Filter")]
    public virtual ICollection<Observation> ObservationFilters { get; set; } = [];

    [InverseProperty("Guider")]
    public virtual ICollection<Observation> ObservationGuiders { get; set; } = [];

    [InverseProperty("Mount")]
    public virtual ICollection<Observation> ObservationMounts { get; set; } = [];

    [InverseProperty("Telescope")]
    public virtual ICollection<Observation> ObservationTelescopes { get; set; } = [];
}
