using Server.Enums;
using Server.Infrastructure.Entities.Refs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("EntityDefinition")]
    public class EntityDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("entity_type")]
        public EntityType EntityType { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("drop_table_id")]
        public Guid? DropTableId { get; set; }
        [Column("resource_type")]
        public ResourceType ResourceType { get; set; }
        [Column("model_asset")]
        public string ModelAsset { get; set; }
        [Column("icon")]
        public string Icon { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid DropTableDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_EntityDefinition_drop_table_id </summary>
        public virtual DropTableDefinitionEntity DropTableDefinition { get; set; }
        /// <summary> Relation Label: FK_MapObjectInstance_entity_definition_id </summary>
        public virtual ICollection<MapObjectInstanceEntity> MapObjectInstances { get; set; } = new HashSet<MapObjectInstanceEntity>();
        /// <summary> Relation Label: FK_MapSpawnPoint_entity_definition_id </summary>
        public virtual ICollection<MapSpawnPointEntity> MapSpawnPoints { get; set; } = new HashSet<MapSpawnPointEntity>();
        /// <summary> Relation Label: FK_CropDefinition_entity_definition_id </summary>
        public virtual ICollection<CropDefinitionEntity> CropDefinitions { get; set; } = new HashSet<CropDefinitionEntity>();
        /// <summary> Relation Label: FK_LivestockDefinition_entity_definition_id </summary>
        public virtual ICollection<LivestockDefinitionEntity> LivestockDefinitions { get; set; } = new HashSet<LivestockDefinitionEntity>();
        /// <summary> Relation Label: FK_MonsterDefinition_entity_definition_id </summary>
        public virtual ICollection<MonsterDefinitionEntity> MonsterDefinitions { get; set; } = new HashSet<MonsterDefinitionEntity>();
        /// <summary> Relation Label: FK_OreDefinition_entity_definition_id </summary>
        public virtual ICollection<OreDefinitionEntity> OreDefinitions { get; set; } = new HashSet<OreDefinitionEntity>();
        /// <summary> Relation Label: FK_LogDefinition_entity_definition_id </summary>
        public virtual ICollection<LogDefinitionEntity> LogDefinitions { get; set; } = new HashSet<LogDefinitionEntity>();
        /// <summary> Relation Label: FK_BuildingDefinition_entity_definition_id </summary>
        public virtual ICollection<BuildingDefinitionEntity> BuildingDefinitions { get; set; } = new HashSet<BuildingDefinitionEntity>();
        /// <summary> Relation Label: FK_NpcDefinition_entity_definition_id </summary>
        public virtual ICollection<NpcDefinitionEntity> NpcDefinitions { get; set; } = new HashSet<NpcDefinitionEntity>();
        /// <summary> Relation Label: FK_PetDefinition_entity_definition_id </summary>
        public virtual ICollection<PetDefinitionEntity> PetDefinitions { get; set; } = new HashSet<PetDefinitionEntity>();
        /// <summary> Relation Label: FK_FishDefinition_entity_definition_id </summary>
        public virtual ICollection<FishDefinitionEntity> FishDefinitions { get; set; } = new HashSet<FishDefinitionEntity>();
        #endregion
    }
}