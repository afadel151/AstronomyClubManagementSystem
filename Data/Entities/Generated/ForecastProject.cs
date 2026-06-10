using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;

[PrimaryKey("ForecastId", "ProjectId")]

public partial class ForecastProject
{
    [Key]
    public int ForecastId { get; set; }

    [Key]
    public int ProjectId { get; set; }

    [StringLength(500)]
    public string? ContributionNotes { get; set; }

    [ForeignKey("ForecastId")]
    [InverseProperty("ForecastProjects")]
    public virtual Forecast Forecast { get; set; } = null!;

    [ForeignKey("ProjectId")]
    [InverseProperty("ForecastProjects")]
    public virtual Project Project { get; set; } = null!;
}
