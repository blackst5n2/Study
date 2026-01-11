using Server.Core.Entities.Definitions;
using Server.Core.Entities.Entities;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Refs
{
    public class MapZone
    {
        public Guid Id { get; set; }
        public Guid MapId { get; set; }
        public string Name { get; set; }
        public MapZoneType ZoneType { get; set; }
        public MapZoneAreaType AreaType { get; set; }
        public string AreaData { get; set; }
        public int Priority { get; set; }
        public Guid? EnvironmentId { get; set; }
        public bool? AllowPvp { get; set; }
        public bool? AllowFarming { get; set; }
        public bool? AllowFishing { get; set; }
        public bool? AllowBuilding { get; set; }
        public string EntryCondition { get; set; }
        public bool Active { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MapZone_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        /// <summary> Relation Label: FK_ZoneEventTrigger_zone_id </summary>
        public virtual ICollection<ZoneEventTrigger> ZoneEventTriggers { get; set; } = new HashSet<ZoneEventTrigger>();
        /// <summary> Relation Label: FK_NpcInstance_zone_id </summary>
        public virtual ICollection<NpcInstance> NpcInstances { get; set; } = new HashSet<NpcInstance>();
        /// <summary> Relation Label: FK_FishingSpotDefinition_zone_id </summary>
        public virtual ICollection<FishingSpotDefinition> FishingSpotDefinitions { get; set; } = new HashSet<FishingSpotDefinition>();
        /// <summary> Relation Label: FK_DungeonZoneLink_map_zone_id </summary>
        public virtual ICollection<DungeonZoneLink> DungeonZoneLinks { get; set; } = new HashSet<DungeonZoneLink>();
        /// <summary> Relation Label: FK_DungeonRun_current_zone_id </summary>
        public virtual ICollection<DungeonRun> DungeonRuns { get; set; } = new HashSet<DungeonRun>();
        #endregion
    }
}