using Microsoft.AspNetCore.Identity;

namespace Data.Entities.Identity;
public class ApplicationUser : IdentityUser
{    public string MemberCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public DateOnly JoinDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public short? BirthYear { get; set; }
    public string MemberStatus { get; set; } = "pending";
    public string? AavsoObserverCode { get; set; }
    public string? Bio { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? Nationality { get; set; }
    public string? City { get; set; }
    public DateTimeOffset? LastLoginAt { get; set; }
    public string? LastLoginIp { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? CreatedBy { get; set; }
    public ApplicationUser? CreatedByUser { get; set; }

    public virtual ICollection<Task> CreatedTasks { get; set; }
    = [];
     public virtual ICollection<EventObservation> CreatedEventObservations { get; set; }
    = [];
     public virtual ICollection<Forecast> CreatedForecasts { get; set; }
    = [];
     public virtual ICollection<Project> CreatedProjects { get; set; }
    = [];
}