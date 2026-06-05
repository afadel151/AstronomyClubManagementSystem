using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Table("OBSERVATION_TYPES")]
[Index("Name", Name = "UK_OT_Name", IsUnique = true)]
public partial class ObservationType
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [InverseProperty("ObservationType")]
    public virtual ICollection<Observation> Observations { get; set; } = new List<Observation>();
}
