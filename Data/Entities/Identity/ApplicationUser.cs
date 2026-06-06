using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Data.Entities.Enums;

namespace Data.Entities.Identity;

[Index(nameof(MemberCode), Name = "UK_USERS_MemberCode", IsUnique = true)]
public class ApplicationUser : IdentityUser
{
    [Required, MaxLength(50)]
    public string MemberCode { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? DisplayName { get; set; }

    public DateOnly JoinDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public short? BirthYear { get; set; }

    public MemberStatusEnum MemberStatus { get; set; } = MemberStatusEnum.Pending;

    [MaxLength(50)]
    public string? AavsoObserverCode { get; set; }

    public string? Bio { get; set; }

    public string? ProfileImageUrl { get; set; }

    [MaxLength(100)]
    public string? Nationality { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    public DateTimeOffset? LastLoginAt { get; set; }
    public string? LastLoginIp { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public string? CreatedBy { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public ApplicationUser? CreatedByUser { get; set; }

    public virtual ICollection<Task> CreatedTasks { get; set; } = [];
    public virtual ICollection<EventObservation> CreatedEventObservations { get; set; } = [];
    public virtual ICollection<Forecast> CreatedForecasts { get; set; } = [];
    public virtual ICollection<Project> CreatedProjects { get; set; } = [];
}