using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("QuestCondition")]
    public class QuestConditionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("quest_id")]
        public Guid QuestId { get; set; }
        [Column("sequence")]
        public int Sequence { get; set; }
        [Column("type")]
        public QuestConditionType Type { get; set; }
        [Column("target_id")]
        public string TargetId { get; set; }
        [Column("required_value")]
        public int RequiredValue { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid QuestDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_QuestCondition_quest_id </summary>
        public virtual QuestDefinitionEntity QuestDefinition { get; set; }
        /// <summary> Relation Label: FK_QuestConditionProgress_quest_condition_id </summary>
        public virtual ICollection<QuestConditionProgressEntity> QuestConditionProgresses { get; set; } = new HashSet<QuestConditionProgressEntity>();
        #endregion
    }
}