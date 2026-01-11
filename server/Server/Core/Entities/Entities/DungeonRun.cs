using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;
using Server.Core.Entities.Refs;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class DungeonRun
    {
        public Guid Id { get; set; }
        public Guid DungeonId { get; set; }
        public Guid? PartyId { get; set; }
        public Guid LeaderId { get; set; }
        public DungeonRunStatus Status { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public Guid? CurrentZoneId { get; set; }
        public float ElapsedTimeSeconds { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_DungeonRun_dungeon_id </summary>
        public virtual DungeonDefinition DungeonDefinition { get; set; }
        /// <summary> Relation Label: FK_DungeonRun_party_id </summary>
        public virtual PartyDefinition PartyDefinition { get; set; }
        /// <summary> Relation Label: FK_DungeonRun_leader_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_DungeonRun_current_zone_id </summary>
        public virtual MapZone MapZone { get; set; }
        /// <summary> Relation Label: FK_DungeonRunParticipant_dungeon_run_id </summary>
        public virtual ICollection<DungeonRunParticipant> DungeonRunParticipants { get; set; } = new HashSet<DungeonRunParticipant>();
        #endregion
    }
}