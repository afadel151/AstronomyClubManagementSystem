using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;

[PrimaryKey("TaskId", "UserId")]
[Index("UserId", Name = "IX_TA_UserId")]
public partial class TaskAssignment
{
    [Key]
    public int TaskId { get; set; }

    [Key]
    public string UserId { get; set; } = null!;

    [StringLength(450)]
    public string AssignedBy { get; set; } = null!;

    public DateTimeOffset AssignedAt { get; set; }

    public bool IsLead { get; set; }

    public DateTimeOffset? ConfirmedAt { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TaskAssignments")]
    public virtual Task Task { get; set; } = null!;
}
