using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;

[Index("Token", Name = "UK_REFRESH_TOKEN", IsUnique = true)]
[Index("UserId", Name = "IX_RT_UserId", IsUnique = false)]
public partial class RefreshToken
{
    [Key]
    public int TokenId { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; } = null!;

    [Required]
    [StringLength(500)]
    public string Token { get; set; } = null!;

    public DateTimeOffset ExpiresAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    [StringLength(45)]
    public string? CreatedByIp { get; set; }

    public DateTimeOffset? RevokedAt { get; set; }

    [StringLength(45)]
    public string? RevokedByIp { get; set; }

    [StringLength(500)]
    public string? ReplacedByToken { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public bool? IsActive { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("RefreshTokens")]
    public virtual ApplicationUser User { get; set; } = null!;
}