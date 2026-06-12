using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;

[Index("IsSolarSystem", Name = "IX_TGT_IsSolarSystem")]
[Index("ObjectTypeCode", Name = "IX_TGT_ObjectTypeCode")]
[Index("RaDeg", "DecDeg", Name = "IX_TGT_RaDec")]
[Index("Code", Name = "UK_TARGETS_Code", IsUnique = true)]
[Index("SimbadId", Name = "UK_TARGETS_SimbadId", IsUnique = true)]
public partial class Target
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(100)]
    public string? SimbadId { get; set; }

    [StringLength(200)]
    public string? CommonName { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MessierId { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? NgcId { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? IcId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? MpcDesignation { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? VsxId { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? HipId { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? GaiaDr3Id { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string ObjectTypeCode { get; set; } = null!;

    [StringLength(100)]
    public string ObjectTypeLabel { get; set; } = null!;

    public bool IsSolarSystem { get; set; }

    [Column(TypeName = "decimal(13, 9)")]
    public decimal? RaDeg { get; set; }

    [Column(TypeName = "decimal(12, 9)")]
    public decimal? DecDeg { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? RaHms { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? DecDms { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal? PmRaMasYr { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal? PmDecMasYr { get; set; }

    [Column(TypeName = "decimal(6, 1)")]
    public decimal? PmEpoch { get; set; }

    [Column(TypeName = "decimal(6, 3)")]
    public decimal? MagnitudeV { get; set; }

    [Column(TypeName = "decimal(6, 3)")]
    public decimal? MagnitudeB { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MagnitudeFilter { get; set; }


    public MagnitudeSystemEnum? MagnitudeSystem { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? Constellation { get; set; }

    [Column(TypeName = "decimal(15, 3)")]
    public decimal? DistanceLy { get; set; }

    [Column(TypeName = "decimal(15, 6)")]
    public decimal? DistancePc { get; set; }

    [Column(TypeName = "decimal(8, 3)")]
    public decimal? AngularSizeArcmin { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? PositionAngleDeg { get; set; }

    [Column(TypeName = "decimal(10, 7)")]
    public decimal? RedshiftZ { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? SpectralType { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? VariabilityType { get; set; }

    [Column(TypeName = "decimal(15, 8)")]
    public decimal? VariabilityPeriodDays { get; set; }

    [Column(TypeName = "decimal(15, 6)")]
    public decimal? EpochMaxJd { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string CatalogSource { get; set; } = null!;

    [StringLength(500)]
    public string? CatalogUrl { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    [InverseProperty("Target")]
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    [InverseProperty("Target")]
    public virtual ICollection<ImageRecord> ImageRecords { get; set; } = new List<ImageRecord>();

    [InverseProperty("Target")]
    public virtual ICollection<Observation> Observations { get; set; } = new List<Observation>();

    [InverseProperty("Target")]
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
