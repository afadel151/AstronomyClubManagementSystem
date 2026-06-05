using Microsoft.AspNetCore.Identity;

namespace Data.Entities.Identity;
public class ApplicationRole : IdentityRole
{    
    public string RoleCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int PermissionLevel { get; set; } = 10;
    public bool CanApproveObservations { get; set; } = false;
    public bool CanManageEquipment { get; set; } = false;
    public bool CanManageMembers { get; set; } = false;
    public bool CanManageProjects { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}