using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("QuestConditionProgress")]
    public class QuestConditionProgressEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("quest_log_id")]
        public Guid QuestLogId { get; set; }
        [Column("quest_condition_id")]
        public Guid QuestConditionId { get; set; }
        [Column("current_value")]
        public int CurrentValue { get; set; }
        [Column("is_completed")]
        public bool IsCompleted { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_QuestConditionProgress_quest_log_id </summary>
        public virtual QuestLogEntity QuestLog { get; set; }
        /// <summary> Relation Label: FK_QuestConditionProgress_quest_condition_id </summary>
        public virtual QuestConditionEntity QuestCondition { get; set; }
        #endregion
    }
}