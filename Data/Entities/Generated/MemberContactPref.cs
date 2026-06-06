using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index("UserId", "Channel", Name = "UK_MCP_UserChannel", IsUnique = true)]
public partial class MemberContactPref
{
    [Key]
    public int PrefId { get; set; }

    public string UserId { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public ContactChannelEnum Channel { get; set; }

    [StringLength(200)]
    public string? ChannelAddress { get; set; }

    public bool IsActive { get; set; }

    [StringLength(500)]
    public string? EventTypes { get; set; }

    public byte? QuietHoursStart { get; set; }

    public byte? QuietHoursEnd { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Timezone { get; set; }
}
