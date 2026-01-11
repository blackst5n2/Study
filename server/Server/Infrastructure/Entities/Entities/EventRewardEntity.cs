using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("EventReward")]
    public class EventRewardEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("event_id")]
        public Guid EventId { get; set; }
        [Column("reward_code")]
        public string RewardCode { get; set; }

        #region Navigation Properties
        public Guid EventDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_EventReward_event_id </summary>
        public virtual EventDefinitionEntity EventDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerEventRewardLog_event_reward_id </summary>
        public virtual ICollection<PlayerEventRewardLogEntity> PlayerEventRewardLogs { get; set; } = new HashSet<PlayerEventRewardLogEntity>();
        #endregion
    }
}