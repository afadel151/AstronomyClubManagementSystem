using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Table("OBSERVATION_SESSIONS")]
[Index("SiteId", Name = "IX_SES_SiteId")]
[Index("StartTimeUtc", Name = "IX_SES_StartTimeUTC", AllDescending = true)]
[Index("Status", Name = "IX_SES_Status")]
[Index("Code", Name = "UK_SES_Code", IsUnique = true)]
public partial class ObservationSession
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(200)]
    public string? Name { get; set; }

    public int SiteId { get; set; }

    [Column("StartTimeUTC")]
    public DateTimeOffset StartTimeUtc { get; set; }

    [Column("EndTimeUTC")]
    public DateTimeOffset? EndTimeUtc { get; set; }

    [Column(TypeName = "decimal(15, 6)")]
    public decimal JulianDateStart { get; set; }

    [Column(TypeName = "decimal(15, 6)")]
    public decimal? JulianDateEnd { get; set; }

    [StringLength(450)]
    public string LeadUserId { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public int SessionTypeId { get; set; }

    public byte? SeeingAntoniadi { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? SeeingFwhmArcsec { get; set; }

    public byte? Transparency { get; set; }

    [Column(TypeName = "decimal(4, 1)")]
    public decimal? LimitingMagVis { get; set; }

    [Column(TypeName = "decimal(6, 3)")]
    public decimal? SqmReading { get; set; }

    [Column(TypeName = "decimal(2, 1)")]
    public decimal? BortleMeasured { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? MoonPhasePct { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? MoonAltDeg { get; set; }

    [Column("MoonsetUTC")]
    public DateTimeOffset? MoonsetUtc { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? TemperatureC { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? HumidityPct { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? WindSpeedKmh { get; set; }

    [Column(TypeName = "decimal(5, 1)")]
    public decimal? WindDirectionDeg { get; set; }

    [Column(TypeName = "decimal(7, 2)")]
    public decimal? PressureHpa { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? DewPointC { get; set; }

    [StringLength(500)]
    public string? WeatherSummary { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    [InverseProperty("Session")]
    public virtual ICollection<Observation> Observations { get; set; } = new List<Observation>();

    [InverseProperty("Session")]
    public virtual ICollection<SessionMember> SessionMembers { get; set; } = new List<SessionMember>();

    [ForeignKey("SessionTypeId")]
    [InverseProperty("ObservationSessions")]
    public virtual ObservationSessionType SessionType { get; set; } = null!;

    [ForeignKey("SiteId")]
    [InverseProperty("ObservationSessions")]
    public virtual ObservationSite Site { get; set; } = null!;

    [InverseProperty("Session")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
