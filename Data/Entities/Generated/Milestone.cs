using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;


[Index("ProjectId", Name = "IX_MS_ProjectId")]
public partial class Milestone
{
    [Key]
    public int Id { get; set; }

    public int ProjectId { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    public DateOnly? DueDate { get; set; }

    public bool IsCompleted { get; set; }

    public DateOnly? CompletionDate { get; set; }

    public short SortOrder { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("Milestones")]
    public virtual Project Project { get; set; } = null!;

    [InverseProperty("Milestone")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
