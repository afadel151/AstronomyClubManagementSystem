using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Table("EVENT_VISIBILITY")]
[Index("EventId", "SiteId", Name = "UK_EV_EventSite", IsUnique = true)]
public partial class EventVisibility
{
    [Key]
    public int Id { get; set; }

    public int EventId { get; set; }

    public int SiteId { get; set; }

    public bool IsVisible { get; set; }

    [Column(TypeName = "decimal(7, 3)")]
    public decimal? MinAltitudeDeg { get; set; }

    [Column(TypeName = "decimal(7, 3)")]
    public decimal? MaxAltitudeDeg { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? BestViewingDirection { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? DurationMinutes { get; set; }

    public DateTimeOffset? RiseTimeUtc { get; set; }

    public DateTimeOffset? SetTimeUtc { get; set; }

    public DateTimeOffset? BestViewingUtc { get; set; }

    [Column(TypeName = "decimal(7, 3)")]
    public decimal? AzimuthAtPeakDeg { get; set; }

    public DateTimeOffset ComputedAt { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("EventVisibilities")]
    public virtual Event Event { get; set; } = null!;

    [ForeignKey("SiteId")]
    [InverseProperty("EventVisibilities")]
    public virtual ObservationSite Site { get; set; } = null!;
}
