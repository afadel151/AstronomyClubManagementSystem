using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;

using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;

[PrimaryKey("ProjectId", "UserId")]

[Index("UserId", Name = "IX_PM_UserId")]
public partial class ProjectMember
{
    [Key]
    public int ProjectId { get; set; }

    [Key]
    public string UserId { get; set; } = null!;

    [StringLength(20)]
    public ProjectMemberRoleEnum Role { get; set; }

    public DateOnly JoinDate { get; set; }

    public DateOnly? LeftDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectMembers")]
    public virtual Project Project { get; set; } = null!;
}
