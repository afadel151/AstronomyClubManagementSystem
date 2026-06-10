using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;

[Index("Name", Name = "UK_TT_Name", IsUnique = true)]
public partial class TaskType
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [InverseProperty("TaskType")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
