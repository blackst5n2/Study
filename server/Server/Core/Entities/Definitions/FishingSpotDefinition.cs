using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Refs;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class FishingSpotDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid MapId { get; set; }
        public Guid? ZoneId { get; set; }
        public MapZoneAreaType AreaType { get; set; }
        public string AreaData { get; set; }
        public int RequiredFishingLevel { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_FishingSpotDefinition_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        /// <summary> Relation Label: FK_FishingSpotDefinition_zone_id </summary>
        public virtual MapZone MapZone { get; set; }
        /// <summary> Relation Label: FK_FishHabitat_fishing_spot_id </summary>
        public virtual ICollection<FishHabitat> FishHabitats { get; set; } = new HashSet<FishHabitat>();
        /// <summary> Relation Label: FK_PlayerFishingLog_fishing_spot_id </summary>
        public virtual ICollection<PlayerFishingLog> PlayerFishingLogs { get; set; } = new HashSet<PlayerFishingLog>();
        #endregion
    }
}