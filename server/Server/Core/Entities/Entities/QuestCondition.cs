using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class QuestCondition
    {
        public Guid Id { get; set; }
        public Guid QuestId { get; set; }
        public int Sequence { get; set; }
        public QuestConditionType Type { get; set; }
        public string TargetId { get; set; }
        public int RequiredValue { get; set; }
        public string Description { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_QuestCondition_quest_id </summary>
        public virtual QuestDefinition QuestDefinition { get; set; }
        /// <summary> Relation Label: FK_QuestConditionProgress_quest_condition_id </summary>
        public virtual ICollection<QuestConditionProgress> QuestConditionProgresses { get; set; } = new HashSet<QuestConditionProgress>();
        #endregion
    }
}