using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Table("EQUIPMENTS")]
[Index("CategoryId", Name = "IX_EQ_CategoryId")]
[Index("Status", Name = "IX_EQ_Status")]
[Index("Code", Name = "UK_EQUIPMENTS_Code", IsUnique = true)]
public partial class Equipment
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Brand { get; set; } = null!;

    [StringLength(150)]
    public string Model { get; set; } = null!;

    public int CategoryId { get; set; }

    [StringLength(50)]
    public string? OpticalDesign { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? SerialNumber { get; set; }

    public DateOnly? PurchaseDate { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? PurchasePrice { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(200)]
    public string? Location { get; set; }

    [StringLength(450)]
    public string? LoanedTo { get; set; }

    public DateOnly? LoanDueDate { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }

    [StringLength(68)]
    [Unicode(false)]
    public string? FitsTelescop { get; set; }

    [StringLength(68)]
    [Unicode(false)]
    public string? FitsInstrume { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Equipment")]
    public virtual EquipmentCategory Category { get; set; } = null!;

    [InverseProperty("Equipment")]
    public virtual ICollection<EquipmentMaintenance> EquipmentMaintenances { get; set; } = new List<EquipmentMaintenance>();

    [InverseProperty("Camera")]
    public virtual ICollection<Observation> ObservationCameras { get; set; } = new List<Observation>();

    [InverseProperty("Filter")]
    public virtual ICollection<Observation> ObservationFilters { get; set; } = new List<Observation>();

    [InverseProperty("Guider")]
    public virtual ICollection<Observation> ObservationGuiders { get; set; } = new List<Observation>();

    [InverseProperty("Mount")]
    public virtual ICollection<Observation> ObservationMounts { get; set; } = new List<Observation>();

    [InverseProperty("Telescope")]
    public virtual ICollection<Observation> ObservationTelescopes { get; set; } = new List<Observation>();
}
