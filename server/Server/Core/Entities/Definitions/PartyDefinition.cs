using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Definitions
{
    public class PartyDefinition
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid LeaderId { get; set; }
        public string LootRule { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DisbandedAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ChatLog_party_id </summary>
        public virtual ICollection<ChatLog> ChatLogs { get; set; } = new HashSet<ChatLog>();
        /// <summary> Relation Label: FK_PartyDefinition_leader_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PartyMember_party_id </summary>
        public virtual ICollection<PartyMember> PartyMembers { get; set; } = new HashSet<PartyMember>();
        /// <summary> Relation Label: FK_DungeonRun_party_id </summary>
        public virtual ICollection<DungeonRun> DungeonRuns { get; set; } = new HashSet<DungeonRun>();
        #endregion
    }
}