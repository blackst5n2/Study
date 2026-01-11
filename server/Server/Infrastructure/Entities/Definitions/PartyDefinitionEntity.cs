using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("PartyDefinition")]
    public class PartyDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("leader_id")]
        public Guid LeaderId { get; set; }
        [Column("loot_rule")]
        public string LootRule { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("disbanded_at")]
        public DateTime? DisbandedAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ChatLog_party_id </summary>
        public virtual ICollection<ChatLogEntity> ChatLogs { get; set; } = new HashSet<ChatLogEntity>();
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PartyDefinition_leader_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_PartyMember_party_id </summary>
        public virtual ICollection<PartyMemberEntity> PartyMembers { get; set; } = new HashSet<PartyMemberEntity>();
        /// <summary> Relation Label: FK_DungeonRun_party_id </summary>
        public virtual ICollection<DungeonRunEntity> DungeonRuns { get; set; } = new HashSet<DungeonRunEntity>();
        #endregion
    }
}