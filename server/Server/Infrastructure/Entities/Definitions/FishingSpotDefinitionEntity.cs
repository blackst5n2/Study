using Server.Enums;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Refs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("FishingSpotDefinition")]
    public class FishingSpotDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("map_id")]
        public Guid MapId { get; set; }
        [Column("zone_id")]
        public Guid? ZoneId { get; set; }
        [Column("area_type")]
        public MapZoneAreaType AreaType { get; set; }
        [Column("area_data")]
        public string AreaData { get; set; }
        [Column("required_fishing_level")]
        public int RequiredFishingLevel { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_FishingSpotDefinition_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        public Guid MapZoneId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_FishingSpotDefinition_zone_id </summary>
        public virtual MapZoneEntity MapZone { get; set; }
        /// <summary> Relation Label: FK_FishHabitat_fishing_spot_id </summary>
        public virtual ICollection<FishHabitatEntity> FishHabitats { get; set; } = new HashSet<FishHabitatEntity>();
        /// <summary> Relation Label: FK_PlayerFishingLog_fishing_spot_id </summary>
        public virtual ICollection<PlayerFishingLogEntity> PlayerFishingLogs { get; set; } = new HashSet<PlayerFishingLogEntity>();
        #endregion
    }
}