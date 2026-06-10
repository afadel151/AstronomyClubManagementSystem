using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;


[Index("Status", Name = "IX_PRJ_Status")]
[Index("TargetId", Name = "IX_PRJ_TargetId")]
[Index("Code", Name = "UK_PRJ_Code", IsUnique = true)]
public partial class Project
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(300)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int ProjectTypeId { get; set; }

    [StringLength(20)]
    public ProjectStatusEnum Status { get; set; }

    [StringLength(10)]
    public ProjectPriorityEnum Priority { get; set; }

    [StringLength(15)]
    public ProjectVisibilityEnum Visibility { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? TargetEndDate { get; set; }

    public DateOnly? ActualEndDate { get; set; }

    [StringLength(450)]
    public string CreatedBy { get; set; } = null!;

    [ForeignKey(nameof(CreatedBy))]
    public virtual ApplicationUser CreatedByUser { get; set; } = null!;

    [StringLength(450)]
    public string? ProjectLeadId { get; set; }

    public int? TargetId { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? TotalIntegrationGoalH { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? TotalIntegrationAchievedH { get; set; }

    [StringLength(500)]
    public string? RepositoryUrl { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    [InverseProperty("Project")]
    public virtual ICollection<ForecastProject> ForecastProjects { get; set; } = new List<ForecastProject>();

    [InverseProperty("Project")]
    public virtual ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();

    [InverseProperty("Project")]
    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();

    [ForeignKey("ProjectTypeId")]
    [InverseProperty("Projects")]
    public virtual ProjectType ProjectType { get; set; } = null!;

    [ForeignKey("TargetId")]
    [InverseProperty("Projects")]
    public virtual Target? Target { get; set; }

    [InverseProperty("Project")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
