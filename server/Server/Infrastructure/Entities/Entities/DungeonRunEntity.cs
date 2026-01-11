using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using Server.Infrastructure.Entities.Refs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("DungeonRun")]
    public class DungeonRunEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("dungeon_id")]
        public Guid DungeonId { get; set; }
        [Column("party_id")]
        public Guid? PartyId { get; set; }
        [Column("leader_id")]
        public Guid LeaderId { get; set; }
        [Column("status")]
        public DungeonRunStatus Status { get; set; }
        [Column("started_at")]
        public DateTime StartedAt { get; set; }
        [Column("ended_at")]
        public DateTime? EndedAt { get; set; }
        [Column("current_zone_id")]
        public Guid? CurrentZoneId { get; set; }
        [Column("elapsed_time_seconds")]
        public float ElapsedTimeSeconds { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid DungeonDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_DungeonRun_dungeon_id </summary>
        public virtual DungeonDefinitionEntity DungeonDefinition { get; set; }
        public Guid PartyDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_DungeonRun_party_id </summary>
        public virtual PartyDefinitionEntity PartyDefinition { get; set; }
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_DungeonRun_leader_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid MapZoneId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_DungeonRun_current_zone_id </summary>
        public virtual MapZoneEntity MapZone { get; set; }
        /// <summary> Relation Label: FK_DungeonRunParticipant_dungeon_run_id </summary>
        public virtual ICollection<DungeonRunParticipantEntity> DungeonRunParticipants { get; set; } = new HashSet<DungeonRunParticipantEntity>();
        #endregion
    }
}