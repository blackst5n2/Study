using Server.Enums;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("BuildingDefinition")]
    public class BuildingDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("entity_definition_id")]
        public Guid EntityDefinitionId { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("type")]
        public BuildingType Type { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("allowed_actions")]
        public string AllowedActions { get; set; }
        [Column("unlock_condition")]
        public string UnlockCondition { get; set; }
        [Column("max_level")]
        public int MaxLevel { get; set; }
        [Column("placement_rules")]
        [Required]
        public string PlacementRules { get; set; }
        [Column("icon")]
        public string Icon { get; set; }
        [Column("model_asset")]
        public string ModelAsset { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_BuildingDefinition_entity_definition_id </summary>
        public virtual EntityDefinitionEntity EntityDefinition { get; set; }
        /// <summary> Relation Label: FK_FarmPlotDefinition_building_definition_id </summary>
        public virtual ICollection<FarmPlotDefinitionEntity> FarmPlotDefinitions { get; set; } = new HashSet<FarmPlotDefinitionEntity>();
        /// <summary> Relation Label: FK_PlayerBuilding_building_definition_id </summary>
        public virtual ICollection<PlayerBuildingEntity> PlayerBuildings { get; set; } = new HashSet<PlayerBuildingEntity>();
        /// <summary> Relation Label: FK_BuildingUpgradeDefinition_building_definition_id </summary>
        public virtual ICollection<BuildingUpgradeDefinitionEntity> BuildingUpgradeDefinitions { get; set; } = new HashSet<BuildingUpgradeDefinitionEntity>();
        #endregion
    }
}