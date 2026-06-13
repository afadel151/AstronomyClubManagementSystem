using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Generated;
public class EquipmentModelCompatibility
{
    public int ModelId { get; set; }
    public int CompatibleWithModelId { get; set; }

    [StringLength(300)]
    public string? Note { get; set; }

    // True = designed exclusively for this pairing
    public bool IsDedicated { get; set; } = false;

    // True = this part ships with / is required by the parent model
    public bool IsIncludedByDefault { get; set; } = false;

    [ForeignKey("ModelId")]
    public virtual EquipmentModel Model { get; set; } = null!;

    [ForeignKey("CompatibleWithModelId")]
    public virtual EquipmentModel CompatibleWithModel { get; set; } = null!;
}