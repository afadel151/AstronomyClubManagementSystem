using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;


[Index("JdMid", Name = "IX_OBS_JdMid")]
[Index("ObserverId", Name = "IX_OBS_ObserverId")]
[Index("Sra", "Sdec", Name = "IX_OBS_RaDec")]
[Index("SessionId", Name = "IX_OBS_SessionId")]
[Index("TargetId", Name = "IX_OBS_TargetId")]
[Index("ObsId", Name = "UK_OBS_ObsId", IsUnique = true)]
public partial class Observation
{
    [Key]
    public int Id { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string ObsId { get; set; } = null!;

    public int SessionId { get; set; }

    public int TargetId { get; set; }

    public int TelescopeId { get; set; }

    public int? CameraId { get; set; }

    public int? FilterId { get; set; }

    public int? MountId { get; set; }

    public int? GuiderId { get; set; }

    public string ObserverId { get; set; } = null!;

    [Column("StartTimeUTC")]
    public DateTimeOffset StartTimeUtc { get; set; }

    [Column("EndTimeUTC")]
    public DateTimeOffset? EndTimeUtc { get; set; }

    [Column(TypeName = "decimal(15, 6)")]
    public decimal JdMid { get; set; }

    [Column(TypeName = "decimal(15, 7)")]
    public decimal? BjdTdb { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public ObservationTimeSystemEnum Timesys { get; set; }

    [Column(TypeName = "decimal(10, 3)")]
    public decimal? ExposureTimeS { get; set; }

    public int ObservationTypeId { get; set; }

    public int DataproductTypeId { get; set; }

    public ImageCalibLevelEnum CalibLevel { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string ObsCollection { get; set; } = null!;

    [Column("SRa", TypeName = "decimal(13, 9)")]
    public decimal Sra { get; set; }

    [Column("SDec", TypeName = "decimal(12, 9)")]
    public decimal Sdec { get; set; }

    [Column("SFov", TypeName = "decimal(8, 5)")]
    public decimal? Sfov { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? AltDeg { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? AzDeg { get; set; }

    [Column(TypeName = "decimal(6, 4)")]
    public decimal? Airmass { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? FilterCode { get; set; }

    [Column(TypeName = "decimal(15, 12)")]
    public decimal? WavelengthMinM { get; set; }

    [Column(TypeName = "decimal(15, 12)")]
    public decimal? WavelengthMaxM { get; set; }

    [Column(TypeName = "decimal(6, 3)")]
    public decimal? MagnitudeMeasured { get; set; }

    [Column(TypeName = "decimal(5, 3)")]
    public decimal? MagnitudeError { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MagnitudeFilter { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public MagnitudeSystemEnum? MagnitudeSystem { get; set; }

    public bool FainterThan { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ComparisonStarId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CheckStarId { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? AavsoChartId { get; set; }

    [Column(TypeName = "decimal(6, 3)")]
    public decimal? GuidingRmsArcsec { get; set; }

    public bool PlateSolved { get; set; }

    public byte? QualityRating { get; set; }

    [StringLength(1000)]
    public string? ResultSummary { get; set; }

    public bool IsPublished { get; set; }

    public DateTimeOffset? PublishedAt { get; set; }

    [StringLength(450)]
    public string? PublishedBy { get; set; }

    [StringLength(200)]
    public string? ObsPublisherDid { get; set; }

    [StringLength(500)]
    public string? AccessUrl { get; set; }

    [StringLength(200)]
    public string? FacilityName { get; set; }

    [StringLength(200)]
    public string? InstrumentName { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    [ForeignKey("CameraId")]
    [InverseProperty("ObservationCameras")]
    public virtual Equipment? Camera { get; set; }

    [ForeignKey("DataproductTypeId")]
    [InverseProperty("Observations")]
    public virtual DataproductType DataproductType { get; set; } = null!;

    [InverseProperty("Observation")]
    public virtual ICollection<EventObservation> EventObservations { get; set; } = new List<EventObservation>();

    [ForeignKey("FilterId")]
    [InverseProperty("ObservationFilters")]
    public virtual Equipment? Filter { get; set; }

    [ForeignKey("GuiderId")]
    [InverseProperty("ObservationGuiders")]
    public virtual Equipment? Guider { get; set; }

    [InverseProperty("Observation")]
    public virtual ICollection<ImageRecord> ImageRecords { get; set; } = new List<ImageRecord>();

    [ForeignKey("MountId")]
    [InverseProperty("ObservationMounts")]
    public virtual Equipment? Mount { get; set; }

    [ForeignKey("ObservationTypeId")]
    [InverseProperty("Observations")]
    public virtual ObservationType ObservationType { get; set; } = null!;

    [ForeignKey("SessionId")]
    [InverseProperty("Observations")]
    public virtual ObservationSession Session { get; set; } = null!;

    [ForeignKey("TargetId")]
    [InverseProperty("Observations")]
    public virtual Target Target { get; set; } = null!;

    [ForeignKey("TelescopeId")]
    [InverseProperty("ObservationTelescopes")]
    public virtual Equipment Telescope { get; set; } = null!;
}
