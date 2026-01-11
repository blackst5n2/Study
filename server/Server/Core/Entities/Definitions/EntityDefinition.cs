using Server.Core.Entities.Refs;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class EntityDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public EntityType EntityType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? DropTableId { get; set; }
        public ResourceType ResourceType { get; set; }
        public string ModelAsset { get; set; }
        public string Icon { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_EntityDefinition_drop_table_id </summary>
        public virtual DropTableDefinition DropTableDefinition { get; set; }
        /// <summary> Relation Label: FK_MapObjectInstance_entity_definition_id </summary>
        public virtual ICollection<MapObjectInstance> MapObjectInstances { get; set; } = new HashSet<MapObjectInstance>();
        /// <summary> Relation Label: FK_MapSpawnPoint_entity_definition_id </summary>
        public virtual ICollection<MapSpawnPoint> MapSpawnPoints { get; set; } = new HashSet<MapSpawnPoint>();
        /// <summary> Relation Label: FK_CropDefinition_entity_definition_id </summary>
        public virtual ICollection<CropDefinition> CropDefinitions { get; set; } = new HashSet<CropDefinition>();
        /// <summary> Relation Label: FK_LivestockDefinition_entity_definition_id </summary>
        public virtual ICollection<LivestockDefinition> LivestockDefinitions { get; set; } = new HashSet<LivestockDefinition>();
        /// <summary> Relation Label: FK_MonsterDefinition_entity_definition_id </summary>
        public virtual ICollection<MonsterDefinition> MonsterDefinitions { get; set; } = new HashSet<MonsterDefinition>();
        /// <summary> Relation Label: FK_OreDefinition_entity_definition_id </summary>
        public virtual ICollection<OreDefinition> OreDefinitions { get; set; } = new HashSet<OreDefinition>();
        /// <summary> Relation Label: FK_LogDefinition_entity_definition_id </summary>
        public virtual ICollection<LogDefinition> LogDefinitions { get; set; } = new HashSet<LogDefinition>();
        /// <summary> Relation Label: FK_BuildingDefinition_entity_definition_id </summary>
        public virtual ICollection<BuildingDefinition> BuildingDefinitions { get; set; } = new HashSet<BuildingDefinition>();
        /// <summary> Relation Label: FK_NpcDefinition_entity_definition_id </summary>
        public virtual ICollection<NpcDefinition> NpcDefinitions { get; set; } = new HashSet<NpcDefinition>();
        /// <summary> Relation Label: FK_PetDefinition_entity_definition_id </summary>
        public virtual ICollection<PetDefinition> PetDefinitions { get; set; } = new HashSet<PetDefinition>();
        /// <summary> Relation Label: FK_FishDefinition_entity_definition_id </summary>
        public virtual ICollection<FishDefinition> FishDefinitions { get; set; } = new HashSet<FishDefinition>();
        #endregion
    }
}