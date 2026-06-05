using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Table("DATAPRODUCT_TYPES")]
[Index("Name", Name = "UK_DPT_Name", IsUnique = true)]
public partial class DataproductType
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [InverseProperty("DataproductType")]
    public virtual ICollection<Observation> Observations { get; set; } = new List<Observation>();
}
