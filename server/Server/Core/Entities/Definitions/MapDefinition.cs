using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;
using Server.Core.Entities.Refs;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class MapDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MapType Type { get; set; }
        public ViewMode ViewMode { get; set; }
        public float SizeX { get; set; }
        public float SizeY { get; set; }
        public string BackgroundAsset { get; set; }
        public bool IsSafeZone { get; set; }
        public Guid? ParentMapId { get; set; }
        public int? MinLevel { get; set; }
        public int? MaxLevel { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MapEnvironment_map_id </summary>
        public virtual ICollection<MapEnvironment> MapEnvironments { get; set; } = new HashSet<MapEnvironment>();
        /// <summary> Relation Label: FK_MapDefinition_parent_map_id </summary>
        public virtual MapDefinition Parent { get; set; }
        /// <summary> Relation Label: FK_MapDefinition_parent_map_id </summary>
        public virtual ICollection<MapDefinition> Children { get; set; } = new HashSet<MapDefinition>();
        /// <summary> Relation Label: FK_MapLayer_map_id </summary>
        public virtual ICollection<MapLayer> MapLayers { get; set; } = new HashSet<MapLayer>();
        /// <summary> Relation Label: FK_MapObjectInstance_map_id </summary>
        public virtual ICollection<MapObjectInstance> MapObjectInstances { get; set; } = new HashSet<MapObjectInstance>();
        /// <summary> Relation Label: FK_MapSpawnPoint_map_id </summary>
        public virtual ICollection<MapSpawnPoint> MapSpawnPoints { get; set; } = new HashSet<MapSpawnPoint>();
        /// <summary> Relation Label: FK_MapZone_map_id </summary>
        public virtual ICollection<MapZone> MapZones { get; set; } = new HashSet<MapZone>();
        /// <summary> Relation Label: FK_EditableArea_map_id </summary>
        public virtual ICollection<EditableArea> EditableAreas { get; set; } = new HashSet<EditableArea>();
        /// <summary> Relation Label: FK_MapDynamicObjectState_map_id </summary>
        public virtual ICollection<MapDynamicObjectState> MapDynamicObjectStates { get; set; } = new HashSet<MapDynamicObjectState>();
        /// <summary> Relation Label: FK_Portal_from_map_id </summary>
        public virtual ICollection<Portal> Portals { get; set; } = new HashSet<Portal>();
        /// <summary> Relation Label: FK_LandPlot_map_id </summary>
        public virtual ICollection<LandPlot> LandPlots { get; set; } = new HashSet<LandPlot>();
        /// <summary> Relation Label: FK_PlayerMonsterKillLog_map_id </summary>
        public virtual ICollection<PlayerMonsterKillLog> PlayerMonsterKillLogs { get; set; } = new HashSet<PlayerMonsterKillLog>();
        /// <summary> Relation Label: FK_NpcInstance_map_id </summary>
        public virtual ICollection<NpcInstance> NpcInstances { get; set; } = new HashSet<NpcInstance>();
        /// <summary> Relation Label: FK_FishingSpotDefinition_map_id </summary>
        public virtual ICollection<FishingSpotDefinition> FishingSpotDefinitions { get; set; } = new HashSet<FishingSpotDefinition>();
        /// <summary> Relation Label: FK_FarmPlotDefinition_map_id </summary>
        public virtual ICollection<FarmPlotDefinition> FarmPlotDefinitions { get; set; } = new HashSet<FarmPlotDefinition>();
        /// <summary> Relation Label: FK_MapEventTrigger_map_id </summary>
        public virtual ICollection<MapEventTrigger> MapEventTriggers { get; set; } = new HashSet<MapEventTrigger>();
        /// <summary> Relation Label: FK_MapPlayerState_map_id </summary>
        public virtual ICollection<MapPlayerState> MapPlayerStates { get; set; } = new HashSet<MapPlayerState>();
        /// <summary> Relation Label: FK_WorldEvent_map_id </summary>
        public virtual ICollection<WorldEvent> WorldEvents { get; set; } = new HashSet<WorldEvent>();
        #endregion
    }
}