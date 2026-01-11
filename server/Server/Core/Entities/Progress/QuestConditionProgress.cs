using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;

namespace Server.Core.Entities.Progress
{
    public class QuestConditionProgress
    {
        public Guid Id { get; set; }
        public Guid QuestLogId { get; set; }
        public Guid QuestConditionId { get; set; }
        public int CurrentValue { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime UpdatedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_QuestConditionProgress_quest_log_id </summary>
        public virtual QuestLog QuestLog { get; set; }
        /// <summary> Relation Label: FK_QuestConditionProgress_quest_condition_id </summary>
        public virtual QuestCondition QuestCondition { get; set; }
        #endregion
    }
}