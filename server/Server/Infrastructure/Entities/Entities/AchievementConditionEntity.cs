using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("AchievementCondition")]
    public class AchievementConditionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("achievement_id")]
        public Guid AchievementId { get; set; }
        [Column("sequence")]
        public int Sequence { get; set; }
        [Column("type")]
        public AchievementConditionType Type { get; set; }
        [Column("target_id")]
        public string TargetId { get; set; }
        [Column("required_value")]
        public long RequiredValue { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid AchievementDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_AchievementCondition_achievement_id </summary>
        public virtual AchievementDefinitionEntity AchievementDefinition { get; set; }
        /// <summary> Relation Label: FK_AchievementConditionProgress_achievement_condition_id </summary>
        public virtual ICollection<AchievementConditionProgressEntity> AchievementConditionProgresses { get; set; } = new HashSet<AchievementConditionProgressEntity>();
        #endregion
    }
}