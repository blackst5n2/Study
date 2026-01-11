using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("BuildingUpgradeDefinition")]
    public class BuildingUpgradeDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("building_definition_id")]
        public Guid BuildingDefinitionId { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("upgrade_effect_description")]
        public string UpgradeEffectDescription { get; set; }
        [Column("upgrade_time_seconds")]
        public int UpgradeTimeSeconds { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_BuildingUpgradeDefinition_building_definition_id </summary>
        public virtual BuildingDefinitionEntity BuildingDefinition { get; set; }
        /// <summary> Relation Label: FK_BuildingUpgradeEffect_upgrade_definition_id </summary>
        public virtual ICollection<BuildingUpgradeEffectEntity> BuildingUpgradeEffects { get; set; } = new HashSet<BuildingUpgradeEffectEntity>();
        #endregion
    }
}