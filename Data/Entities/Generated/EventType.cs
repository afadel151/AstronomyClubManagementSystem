using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;


[Index("Name", Name = "UK_ET_Name", IsUnique = true)]
public partial class EventType
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [InverseProperty("EventType")]
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
