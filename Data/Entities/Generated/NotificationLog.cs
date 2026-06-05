using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Table("NOTIFICATION_LOG")]
public partial class NotificationLog
{
    [Key]
    public int NotificationId { get; set; }

    [StringLength(450)]
    public string RecipientId { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Channel { get; set; } = null!;

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
    public string Status { get; set; } = null!;

    public DateTimeOffset? ScheduledFor { get; set; }

    public DateTimeOffset? SentAt { get; set; }

    public byte RetryCount { get; set; }

    [StringLength(1000)]
    public string? LastError { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
