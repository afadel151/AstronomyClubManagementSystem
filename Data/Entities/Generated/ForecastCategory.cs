using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;

[Index("Name", Name = "UK_FC_Name", IsUnique = true)]
public partial class ForecastCategory
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Forecast> Forecasts { get; set; } = new List<Forecast>();
}
