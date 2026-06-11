using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;

public partial class EquipmentUpload
{
    [Key]
    public int Id { get; set; }
    public int EquipmentId {get;set;}

    [Required, StringLength(500), Unicode(false)]
    public string ObjectKey {get; set;} = "";

    [StringLength(200)]
    public string? Caption { get; set; }
   public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    [ForeignKey("EquipmentId")]
    [InverseProperty("EquipmentUploads")]
    public virtual Equipment Equipment {get;set;}  = null!;
}