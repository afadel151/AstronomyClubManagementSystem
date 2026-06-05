using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Table("OBSERVATION_SITES")]
[Index("Code", Name = "UK_OS_Code", IsUnique = true)]
public partial class ObservationSite
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(30)]
    public string SiteType { get; set; } = null!;

    [Column(TypeName = "decimal(9, 6)")]
    public decimal LatitudeDeg { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal LongitudeDeg { get; set; }

    [Column(TypeName = "decimal(6, 1)")]
    public decimal AltitudeInMeters { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TimeZone { get; set; } = null!;

    [Column(TypeName = "decimal(2, 1)")]
    public decimal? BortleClass { get; set; }

    [Column(TypeName = "decimal(6, 3)")]
    public decimal? SqmAvg { get; set; }

    [Column(TypeName = "decimal(4, 1)")]
    public decimal? LimitingMagAvg { get; set; }

    [StringLength(500)]
    public string? HorizonProfileUrl { get; set; }

    [StringLength(1000)]
    public string? LightPollutionNotes { get; set; }

    [StringLength(1000)]
    public string? AccessNotes { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    [InverseProperty("Site")]
    public virtual ICollection<EventVisibility> EventVisibilities { get; set; } = new List<EventVisibility>();

    [InverseProperty("Site")]
    public virtual ICollection<ObservationSession> ObservationSessions { get; set; } = new List<ObservationSession>();
}
