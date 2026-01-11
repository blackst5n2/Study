using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("AchievementConditionProgress")]
    public class AchievementConditionProgressEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("achievement_log_id")]
        public Guid AchievementLogId { get; set; }
        [Column("achievement_condition_id")]
        public Guid AchievementConditionId { get; set; }
        [Column("current_value")]
        public long CurrentValue { get; set; }
        [Column("is_completed")]
        public bool IsCompleted { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_AchievementConditionProgress_achievement_log_id </summary>
        public virtual AchievementLogEntity AchievementLog { get; set; }
        /// <summary> Relation Label: FK_AchievementConditionProgress_achievement_condition_id </summary>
        public virtual AchievementConditionEntity AchievementCondition { get; set; }
        #endregion
    }
}