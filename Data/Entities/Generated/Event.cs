using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index("PeakDateUtc", Name = "IX_EV_PeakDate")]
[Index("Code", Name = "UK_EV_Code", IsUnique = true)]
public partial class Event
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(300)]
    public string Name { get; set; } = null!;

    public int EventTypeId { get; set; }

    public int? TargetId { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    public DateTimeOffset PeakDateUtc { get; set; }

    [Column(TypeName = "decimal(15, 6)")]
    public decimal JdPeak { get; set; }

    public DateTimeOffset? EventStartUtc { get; set; }

    public DateTimeOffset? EventEndUtc { get; set; }

    [StringLength(30)]
    public EventGlobalVisibilityEnum? VisibilityGlobal { get; set; }

    [Column(TypeName = "decimal(13, 9)")]
    public decimal? RaPeakDeg { get; set; }

    [Column(TypeName = "decimal(12, 9)")]
    public decimal? DecPeakDeg { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? MagnitudeAtPeak { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? DurationMinutes { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? Constellation { get; set; }

    [StringLength(100)]
    public string? Source { get; set; }

    [StringLength(500)]
    public string? SourceUrl { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? MpcDesignation { get; set; }

    public bool? IsVisibleFromSite { get; set; }

    public bool AlertSent { get; set; }

    public byte AlertDaysBefore { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    [InverseProperty("Event")]
    public virtual ICollection<EventObservation> EventObservations { get; set; } = new List<EventObservation>();

    [ForeignKey("EventTypeId")]
    [InverseProperty("Events")]
    public virtual EventType EventType { get; set; } = null!;

    [InverseProperty("Event")]
    public virtual ICollection<EventVisibility> EventVisibilities { get; set; } = new List<EventVisibility>();

    [ForeignKey("TargetId")]
    [InverseProperty("Events")]
    public virtual Target? Target { get; set; }
}
