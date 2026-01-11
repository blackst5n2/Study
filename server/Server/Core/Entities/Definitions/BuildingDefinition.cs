using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class BuildingDefinition
    {
        public Guid Id { get; set; }
        public Guid EntityDefinitionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public BuildingType Type { get; set; }
        public string Description { get; set; }
        public string AllowedActions { get; set; }
        public string UnlockCondition { get; set; }
        public int MaxLevel { get; set; }
        public string PlacementRules { get; set; }
        public string Icon { get; set; }
        public string ModelAsset { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_BuildingDefinition_entity_definition_id </summary>
        public virtual EntityDefinition EntityDefinition { get; set; }
        /// <summary> Relation Label: FK_FarmPlotDefinition_building_definition_id </summary>
        public virtual ICollection<FarmPlotDefinition> FarmPlotDefinitions { get; set; } = new HashSet<FarmPlotDefinition>();
        /// <summary> Relation Label: FK_PlayerBuilding_building_definition_id </summary>
        public virtual ICollection<PlayerBuilding> PlayerBuildings { get; set; } = new HashSet<PlayerBuilding>();
        /// <summary> Relation Label: FK_BuildingUpgradeDefinition_building_definition_id </summary>
        public virtual ICollection<BuildingUpgradeDefinition> BuildingUpgradeDefinitions { get; set; } = new HashSet<BuildingUpgradeDefinition>();
        #endregion
    }
}