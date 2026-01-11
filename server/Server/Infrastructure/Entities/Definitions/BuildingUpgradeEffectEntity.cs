using Server.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("BuildingUpgradeEffect")]
    public class BuildingUpgradeEffectEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("upgrade_definition_id")]
        public Guid UpgradeDefinitionId { get; set; }
        [Column("effect_type")]
        public BuildingEffectType EffectType { get; set; }
        [Column("effect_key")]
        public string EffectKey { get; set; }
        [Column("value")]
        public string Value { get; set; }
        [Column("extra_data")]
        public string ExtraData { get; set; }

        #region Navigation Properties
        public Guid BuildingUpgradeDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_BuildingUpgradeEffect_upgrade_definition_id </summary>
        public virtual BuildingUpgradeDefinitionEntity BuildingUpgradeDefinition { get; set; }
        #endregion
    }
}