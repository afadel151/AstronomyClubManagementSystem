using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index("ActionDate", Name = "IX_MRA_Date", AllDescending = true)]
[Index("RoleId", Name = "IX_MRA_RoleId")]
[Index("UserId", Name = "IX_MRA_UserId")]
public partial class MemberRoleAudit
{
    [Key]
    public int AuditId { get; set; }

    public string UserId { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public MemberRoleAuditActionEnum Action { get; set; }

    public DateTimeOffset ActionDate { get; set; }

    [StringLength(450)]
    public string ActionBy { get; set; } = null!;

    public DateOnly? ExpiryDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}
