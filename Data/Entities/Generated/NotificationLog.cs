using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;

public partial class NotificationLog
{
    [Key]
    public int NotificationId { get; set; }

    [StringLength(450)]
    public string RecipientId { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public ContactChannelEnum Channel { get; set; }

    [StringLength(200)]
    public string ChannelAddress { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string EventType { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string? EntityType { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string? EntityCode { get; set; }

    [StringLength(300)]
    public string? Subject { get; set; }

    [StringLength(4000)]
    public string Body { get; set; } = null!;

    public string? PayloadJson { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public NotificationStatusEnum Status { get; set; }

    public DateTimeOffset? ScheduledFor { get; set; }

    public DateTimeOffset? SentAt { get; set; }

    public byte RetryCount { get; set; }

    [StringLength(1000)]
    public string? LastError { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
