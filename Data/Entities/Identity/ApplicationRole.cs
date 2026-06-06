using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Identity;

[Index(nameof(RoleCode), Name = "UK_ROLES_RoleCode", IsUnique = true)]
public class ApplicationRole : IdentityRole
{
    [Required, MaxLength(50)]
    public string RoleCode { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public int PermissionLevel { get; set; } = 10;

    public bool CanApproveObservations { get; set; } = false;
    public bool CanManageEquipment { get; set; } = false;
    public bool CanManageMembers { get; set; } = false;
    public bool CanManageProjects { get; set; } = false;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}