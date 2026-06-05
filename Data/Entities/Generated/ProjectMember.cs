using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[PrimaryKey("ProjectId", "UserId")]
[Table("PROJECT_MEMBERS")]
[Index("UserId", Name = "IX_PM_UserId")]
public partial class ProjectMember
{
    [Key]
    public int ProjectId { get; set; }

    [Key]
    public string UserId { get; set; } = null!;

    [StringLength(20)]
    public string Role { get; set; } = null!;

    public DateOnly JoinDate { get; set; }

    public DateOnly? LeftDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectMembers")]
    public virtual Project Project { get; set; } = null!;
}
