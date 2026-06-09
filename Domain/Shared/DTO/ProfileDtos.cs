using System.ComponentModel.DataAnnotations;

namespace Domain.Shared.DTO;

public sealed record ProfileDetailsDto
{
    public string Id { get; init; } = string.Empty;
    public string MemberCode { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? DisplayName { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public DateOnly JoinDate { get; init; }
    public short? BirthYear { get; init; }
    public string MemberStatus { get; init; } = string.Empty;
    public string? AavsoObserverCode { get; init; }
    public string? Bio { get; init; }
    public string? ProfileImageUrl { get; init; }
    public string? Nationality { get; init; }
    public string? City { get; init; }
    public DateTimeOffset? LastLoginAt { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
    public IReadOnlyList<string> Roles { get; init; } = [];
}

public sealed record UpdateProfileRequest
{
    [Required, MaxLength(200)]
    public string FullName { get; init; } = string.Empty;

    [MaxLength(100)]
    public string? DisplayName { get; init; }

    [Phone, MaxLength(50)]
    public string? PhoneNumber { get; init; }

    [Range(1900, 9999)]
    public short? BirthYear { get; init; }

    [MaxLength(50)]
    public string? AavsoObserverCode { get; init; }

    [MaxLength(4000)]
    public string? Bio { get; init; }

    [Url, MaxLength(1000)]
    public string? ProfileImageUrl { get; init; }

    [MaxLength(100)]
    public string? Nationality { get; init; }

    [MaxLength(100)]
    public string? City { get; init; }
}
