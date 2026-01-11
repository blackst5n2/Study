using Server.Core.Entities.Entities;
using Server.Core.Entities.Refs;

namespace Server.Core.Entities.Definitions
{
    public class DungeonDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? EntryPortalId { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int LevelRequirement { get; set; }
        public string RewardPreview { get; set; }
        public bool IsRandom { get; set; }
        public bool HasBoss { get; set; }
        public int? TimeLimitSeconds { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_DungeonDefinition_entry_portal_id </summary>
        public virtual Portal Portal { get; set; }
        /// <summary> Relation Label: FK_DungeonZoneLink_dungeon_id </summary>
        public virtual ICollection<DungeonZoneLink> DungeonZoneLinks { get; set; } = new HashSet<DungeonZoneLink>();
        /// <summary> Relation Label: FK_DungeonRun_dungeon_id </summary>
        public virtual ICollection<DungeonRun> DungeonRuns { get; set; } = new HashSet<DungeonRun>();
        #endregion
    }
}