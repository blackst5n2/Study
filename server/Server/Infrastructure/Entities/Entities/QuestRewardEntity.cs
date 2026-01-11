using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("QuestReward")]
    public class QuestRewardEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("quest_id")]
        public Guid QuestId { get; set; }
        [Column("type")]
        public QuestRewardType Type { get; set; }
        [Column("target_id")]
        public string TargetId { get; set; }
        [Column("value")]
        public long Value { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid QuestDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_QuestReward_quest_id </summary>
        public virtual QuestDefinitionEntity QuestDefinition { get; set; }
        /// <summary> Relation Label: FK_QuestRewardLog_quest_reward_id </summary>
        public virtual ICollection<QuestRewardLogEntity> QuestRewardLogs { get; set; } = new HashSet<QuestRewardLogEntity>();
        #endregion
    }
}