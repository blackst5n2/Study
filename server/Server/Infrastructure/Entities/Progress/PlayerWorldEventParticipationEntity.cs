using Server.Infrastructure.Entities.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerWorldEventParticipation")]
    public class PlayerWorldEventParticipationEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("event_id")]
        public Guid EventId { get; set; }
        [Column("joined_at")]
        public DateTime JoinedAt { get; set; }
        [Column("progress")]
        public string Progress { get; set; }
        [Column("reward_claimed")]
        public bool RewardClaimed { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerWorldEventParticipation_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid WorldEventId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerWorldEventParticipation_event_id </summary>
        public virtual WorldEventEntity WorldEvent { get; set; }
        #endregion
    }
}