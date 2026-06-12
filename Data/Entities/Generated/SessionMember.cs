using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;

[PrimaryKey("SessionId", "UserId")]
[Index("UserId", Name = "IX_SM_UserId")]
public partial class SessionMember
{
    [Key]
    public int SessionId { get; set; }

    [Key]
    public string UserId { get; set; } = null!;


    public SessionMemberRoleEnum SessionRole { get; set; }

    public DateTimeOffset? ArrivalTimeUtc { get; set; }

    public DateTimeOffset? DepartureTimeUtc { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [ForeignKey("SessionId")]
    [InverseProperty("SessionMembers")]
    public virtual ObservationSession Session { get; set; } = null!;
}
