using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;


[Index("ObservationId", Name = "IX_IR_ObservationId")]
[Index("PublicationStatus", Name = "IX_IR_PubStatus")]
[Index("TargetId", Name = "IX_IR_TargetId")]
[Index("Code", Name = "UK_IR_Code", IsUnique = true)]
public partial class ImageRecord
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    public int ObservationId { get; set; }

    public int TargetId { get; set; }

    [StringLength(450)]
    public string CapturedBy { get; set; } = null!;

    [StringLength(450)]
    public string? ProcessedBy { get; set; }

    [StringLength(20)]
    public ImageTypeEnum ImageType { get; set; }

    public DateTimeOffset CaptureDateUtc { get; set; }

    public ImageCalibLevelEnum CalibLevel { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? FilterCode { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? TotalIntegrationS { get; set; }

    public int? FrameCount { get; set; }

    [StringLength(20)]
    public ImagePublicationStatusEnum PublicationStatus { get; set; }

    public bool IsShowcase { get; set; }

    [StringLength(500)]
    public string? ThumbnailUrl { get; set; }

    [StringLength(500)]
    public string? PreviewUrl { get; set; }

    [StringLength(500)]
    public string? FitsUrl { get; set; }

    public long? FileSizeBytes { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    [ForeignKey("ObservationId")]
    [InverseProperty("ImageRecords")]
    public virtual Observation Observation { get; set; } = null!;

    [ForeignKey("TargetId")]
    [InverseProperty("ImageRecords")]
    public virtual Target Target { get; set; } = null!;
}
