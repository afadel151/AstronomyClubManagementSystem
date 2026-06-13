using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;

[Index("CategoryId", Name = "IX_EQ_CategoryId")]

public partial class EquipmentModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(120)]
    [Unicode(false)]
    public string Slug { get; set; } = null!;

    public int CategoryId { get; set; }
    public int BrandId { get; set; }

    [StringLength(68)]
    [Unicode(false)]
    public string? FitsTelescop { get; set; }
    public string? Url { get; set; }

    [StringLength(68)]
    [Unicode(false)]
    public string? FitsInstrume { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? Specifications { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    [InverseProperty("EquipmentModel")]
    public virtual ICollection<Equipment> Equipments { get; set; } = [];

    [ForeignKey("CategoryId")]
    [InverseProperty("EquipmentModels")]
    public virtual EquipmentCategory EquipmentCategory { get; set; } = null!;

    [ForeignKey("BrandId")]
    [InverseProperty("EquipmentModels")]
    public virtual EquipmentBrand EquipmentBrand { get; set; } = null!;

    [InverseProperty("Model")]
    public virtual ICollection<EquipmentModelCompatibility> Compatibilities { get; set; } = [];

    [InverseProperty("CompatibleWithModel")]
    public virtual ICollection<EquipmentModelCompatibility> CompatibleWith { get; set; } = [];
}