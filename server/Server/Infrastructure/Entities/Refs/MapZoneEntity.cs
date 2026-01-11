using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Refs
{
    [Table("MapZone")]
    public class MapZoneEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("map_id")]
        public Guid MapId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("zone_type")]
        public MapZoneType ZoneType { get; set; }
        [Column("area_type")]
        public MapZoneAreaType AreaType { get; set; }
        [Column("area_data")]
        public string AreaData { get; set; }
        [Column("priority")]
        public int Priority { get; set; }
        [Column("environment_id")]
        public Guid? EnvironmentId { get; set; }
        [Column("allow_pvp")]
        public bool? AllowPvp { get; set; }
        [Column("allow_farming")]
        public bool? AllowFarming { get; set; }
        [Column("allow_fishing")]
        public bool? AllowFishing { get; set; }
        [Column("allow_building")]
        public bool? AllowBuilding { get; set; }
        [Column("entry_condition")]
        public string EntryCondition { get; set; }
        [Column("active")]
        public bool Active { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MapZone_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        /// <summary> Relation Label: FK_ZoneEventTrigger_zone_id </summary>
        public virtual ICollection<ZoneEventTriggerEntity> ZoneEventTriggers { get; set; } = new HashSet<ZoneEventTriggerEntity>();
        /// <summary> Relation Label: FK_NpcInstance_zone_id </summary>
        public virtual ICollection<NpcInstanceEntity> NpcInstances { get; set; } = new HashSet<NpcInstanceEntity>();
        /// <summary> Relation Label: FK_FishingSpotDefinition_zone_id </summary>
        public virtual ICollection<FishingSpotDefinitionEntity> FishingSpotDefinitions { get; set; } = new HashSet<FishingSpotDefinitionEntity>();
        /// <summary> Relation Label: FK_DungeonZoneLink_map_zone_id </summary>
        public virtual ICollection<DungeonZoneLinkEntity> DungeonZoneLinks { get; set; } = new HashSet<DungeonZoneLinkEntity>();
        /// <summary> Relation Label: FK_DungeonRun_current_zone_id </summary>
        public virtual ICollection<DungeonRunEntity> DungeonRuns { get; set; } = new HashSet<DungeonRunEntity>();
        #endregion
    }
}