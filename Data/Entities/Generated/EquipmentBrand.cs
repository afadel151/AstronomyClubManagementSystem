using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;
[Index(nameof(Name), Name = "UK_EQUIPMENTBRAND_Name", IsUnique = true)]
[Index(nameof(Slug), Name = "UK_EQUIPMENTBRAND_Slug", IsUnique = true)]
public partial class EquipmentBrand
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

    [StringLength(100)]
    public string? CountryOfOrigin { get; set; }
    [StringLength(500)]
    public string? LogoUrl { get; set; }


    [StringLength(1000)]
    public string? Notes { get; set; }


    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    
    [InverseProperty("EquipmentBrand")]
    public virtual ICollection<EquipmentModel> EquipmentModels { get; set; } = [];
}