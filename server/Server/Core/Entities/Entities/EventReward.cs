using Server.Core.Entities.Definitions;
using Server.Core.Entities.Logs;

namespace Server.Core.Entities.Entities
{
    public class EventReward
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string RewardCode { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_EventReward_event_id </summary>
        public virtual EventDefinition EventDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerEventRewardLog_event_reward_id </summary>
        public virtual ICollection<PlayerEventRewardLog> PlayerEventRewardLogs { get; set; } = new HashSet<PlayerEventRewardLog>();
        #endregion
    }
}