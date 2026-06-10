using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Generated;
public class EquipmentCompatibility
{
    public int EquipmentId { get; set; }          
    public int CompatibleWithId { get; set; }     

    [StringLength(50)]
    public string? CompatibilityNote { get; set; } 

    [ForeignKey("EquipmentId")]
    public virtual Equipment Accessory { get; set; } = null!;

    [ForeignKey("CompatibleWithId")]
    public virtual Equipment CompatibleEquipment { get; set; } = null!;
}