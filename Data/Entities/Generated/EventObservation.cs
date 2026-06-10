using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;

[PrimaryKey("EventId", "ObservationId")]

public partial class EventObservation
{
    [Key]
    public int EventId { get; set; }

    [Key]
    public int ObservationId { get; set; }

    [StringLength(450)]
    public string CreatedBy { get; set; } = null!;

    [ForeignKey(nameof(CreatedBy))]
    public virtual ApplicationUser CreatedByUser { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("EventObservations")]
    public virtual Event Event { get; set; } = null!;

    [ForeignKey("ObservationId")]
    [InverseProperty("EventObservations")]
    public virtual Observation Observation { get; set; } = null!;
}
