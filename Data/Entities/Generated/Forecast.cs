using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index("PeriodYear", Name = "IX_FCST_PeriodYear")]
[Index("Status", Name = "IX_FCST_Status")]
[Index("Code", Name = "UK_FORECASTS_Code", IsUnique = true)]
public partial class Forecast
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(300)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    [StringLength(20)]
    public ForecastStatusEnum Status { get; set; }

    public short PeriodYear { get; set; }

    public DateOnly? TargetDate { get; set; }

    public DateOnly? AchievedDate { get; set; }

    [StringLength(450)]
    public string CreatedBy { get; set; } = null!;


    [ForeignKey(nameof(CreatedBy))]
    public virtual ApplicationUser CreatedByUser { get; set; } = null!;

    [StringLength(450)]
    public string? ApprovedBy { get; set; }

    [StringLength(500)]
    public string? SuccessMetric { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Forecasts")]
    public virtual ForecastCategory Category { get; set; } = null!;

    [InverseProperty("Forecast")]
    public virtual ICollection<ForecastProject> ForecastProjects { get; set; } = new List<ForecastProject>();
}
