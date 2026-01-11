using Server.Enums;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using Server.Infrastructure.Entities.Refs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("MapDefinition")]
    public class MapDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("type")]
        public MapType Type { get; set; }
        [Column("view_mode")]
        public ViewMode ViewMode { get; set; }
        [Column("size_x")]
        public float SizeX { get; set; }
        [Column("size_y")]
        public float SizeY { get; set; }
        [Column("background_asset")]
        public string BackgroundAsset { get; set; }
        [Column("is_safe_zone")]
        public bool IsSafeZone { get; set; }
        [Column("parent_map_id")]
        public Guid? ParentMapId { get; set; }
        [Column("min_level")]
        public int? MinLevel { get; set; }
        [Column("max_level")]
        public int? MaxLevel { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MapEnvironment_map_id </summary>
        public virtual ICollection<MapEnvironmentEntity> MapEnvironments { get; set; } = new HashSet<MapEnvironmentEntity>();
        public Guid ParentId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MapDefinition_parent_map_id </summary>
        public virtual MapDefinitionEntity Parent { get; set; }
        /// <summary> Relation Label: FK_MapDefinition_parent_map_id </summary>
        public virtual ICollection<MapDefinitionEntity> Children { get; set; } = new HashSet<MapDefinitionEntity>();
        /// <summary> Relation Label: FK_MapLayer_map_id </summary>
        public virtual ICollection<MapLayerEntity> MapLayers { get; set; } = new HashSet<MapLayerEntity>();
        /// <summary> Relation Label: FK_MapObjectInstance_map_id </summary>
        public virtual ICollection<MapObjectInstanceEntity> MapObjectInstances { get; set; } = new HashSet<MapObjectInstanceEntity>();
        /// <summary> Relation Label: FK_MapSpawnPoint_map_id </summary>
        public virtual ICollection<MapSpawnPointEntity> MapSpawnPoints { get; set; } = new HashSet<MapSpawnPointEntity>();
        /// <summary> Relation Label: FK_MapZone_map_id </summary>
        public virtual ICollection<MapZoneEntity> MapZones { get; set; } = new HashSet<MapZoneEntity>();
        /// <summary> Relation Label: FK_EditableArea_map_id </summary>
        public virtual ICollection<EditableAreaEntity> EditableAreas { get; set; } = new HashSet<EditableAreaEntity>();
        /// <summary> Relation Label: FK_MapDynamicObjectState_map_id </summary>
        public virtual ICollection<MapDynamicObjectStateEntity> MapDynamicObjectStates { get; set; } = new HashSet<MapDynamicObjectStateEntity>();
        /// <summary> Relation Label: FK_Portal_from_map_id </summary>
        public virtual ICollection<PortalEntity> Portals { get; set; } = new HashSet<PortalEntity>();
        /// <summary> Relation Label: FK_LandPlot_map_id </summary>
        public virtual ICollection<LandPlotEntity> LandPlots { get; set; } = new HashSet<LandPlotEntity>();
        /// <summary> Relation Label: FK_PlayerMonsterKillLog_map_id </summary>
        public virtual ICollection<PlayerMonsterKillLogEntity> PlayerMonsterKillLogs { get; set; } = new HashSet<PlayerMonsterKillLogEntity>();
        /// <summary> Relation Label: FK_NpcInstance_map_id </summary>
        public virtual ICollection<NpcInstanceEntity> NpcInstances { get; set; } = new HashSet<NpcInstanceEntity>();
        /// <summary> Relation Label: FK_FishingSpotDefinition_map_id </summary>
        public virtual ICollection<FishingSpotDefinitionEntity> FishingSpotDefinitions { get; set; } = new HashSet<FishingSpotDefinitionEntity>();
        /// <summary> Relation Label: FK_FarmPlotDefinition_map_id </summary>
        public virtual ICollection<FarmPlotDefinitionEntity> FarmPlotDefinitions { get; set; } = new HashSet<FarmPlotDefinitionEntity>();
        /// <summary> Relation Label: FK_MapEventTrigger_map_id </summary>
        public virtual ICollection<MapEventTriggerEntity> MapEventTriggers { get; set; } = new HashSet<MapEventTriggerEntity>();
        /// <summary> Relation Label: FK_MapPlayerState_map_id </summary>
        public virtual ICollection<MapPlayerStateEntity> MapPlayerStates { get; set; } = new HashSet<MapPlayerStateEntity>();
        /// <summary> Relation Label: FK_WorldEvent_map_id </summary>
        public virtual ICollection<WorldEventEntity> WorldEvents { get; set; } = new HashSet<WorldEventEntity>();
        #endregion
    }
}