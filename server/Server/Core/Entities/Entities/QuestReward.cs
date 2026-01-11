using Server.Core.Entities.Definitions;
using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class QuestReward
    {
        public Guid Id { get; set; }
        public Guid QuestId { get; set; }
        public QuestRewardType Type { get; set; }
        public string TargetId { get; set; }
        public long Value { get; set; }
        public string Description { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_QuestReward_quest_id </summary>
        public virtual QuestDefinition QuestDefinition { get; set; }
        /// <summary> Relation Label: FK_QuestRewardLog_quest_reward_id </summary>
        public virtual ICollection<QuestRewardLog> QuestRewardLogs { get; set; } = new HashSet<QuestRewardLog>();
        #endregion
    }
}