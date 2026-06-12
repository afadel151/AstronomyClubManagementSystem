using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Generated;
public class EquipmentModelCompatibility
{
    public int AccessoryId { get; set; }       
    public int CompatibleWithId { get; set; }     

    [StringLength(50)]
    public string? CompatibilityNote { get; set; } 
    public bool IsDedicated { get; set; } = false;

    [ForeignKey("AccessoryId")]
    public virtual EquipmentModel Accessory { get; set; } = null!;

    [ForeignKey("CompatibleWithId")]
    public virtual EquipmentModel CompatibleWith { get; set; } = null!;
}