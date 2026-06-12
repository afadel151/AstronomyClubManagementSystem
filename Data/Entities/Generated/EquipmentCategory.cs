using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;


[Index("Name", Name = "UK_EQUIPMENT_CATEGORY_Name", IsUnique = true)]
public partial class EquipmentCategory
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public bool Accessory { get; set; } = false;
    [StringLength(500)]
    public string? Description { get; set; }

    public SpecsTypeEnum SpecsType { get; set; } = SpecsTypeEnum.None;

    [InverseProperty("EquipmentCategory")]
    public virtual ICollection<EquipmentModel> EquipmentModels { get; set; } = [];
}
