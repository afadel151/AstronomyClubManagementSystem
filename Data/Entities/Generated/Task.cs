using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Table("TASKS")]
[Index("DueDate", Name = "IX_TSK_DueDate")]
[Index("ProjectId", Name = "IX_TSK_ProjectId")]
[Index("Status", Name = "IX_TSK_Status")]
[Index("TaskCode", Name = "UK_TSK_Code", IsUnique = true)]
public partial class Task
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string TaskCode { get; set; } = null!;

    public int ProjectId { get; set; }

    public int? MilestoneId { get; set; }

    public int? ParentTaskId { get; set; }

    [StringLength(300)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int TaskTypeId { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(10)]
    public string Priority { get; set; } = null!;

    public DateOnly? DueDate { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? EstimatedHours { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? ActualHours { get; set; }

    [StringLength(450)]
    public string CreatedBy { get; set; } = null!;


    [ForeignKey(nameof(CreatedBy))]
    public virtual ApplicationUser CreatedByUser { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public int? SessionId { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; }

    [InverseProperty("ParentTask")]
    public virtual ICollection<Task> InverseParentTask { get; set; } = new List<Task>();

    [ForeignKey("MilestoneId")]
    [InverseProperty("Tasks")]
    public virtual Milestone? Milestone { get; set; }

    [ForeignKey("ParentTaskId")]
    [InverseProperty("InverseParentTask")]
    public virtual Task? ParentTask { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("Tasks")]
    public virtual Project Project { get; set; } = null!;

    [ForeignKey("SessionId")]
    [InverseProperty("Tasks")]
    public virtual ObservationSession? Session { get; set; }

    [InverseProperty("Task")]
    public virtual ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();

    [ForeignKey("TaskTypeId")]
    [InverseProperty("Tasks")]
    public virtual TaskType TaskType { get; set; } = null!;
}
