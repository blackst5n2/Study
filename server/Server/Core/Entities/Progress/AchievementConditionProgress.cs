using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;

namespace Server.Core.Entities.Progress
{
    public class AchievementConditionProgress
    {
        public Guid Id { get; set; }
        public Guid AchievementLogId { get; set; }
        public Guid AchievementConditionId { get; set; }
        public long CurrentValue { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime UpdatedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_AchievementConditionProgress_achievement_log_id </summary>
        public virtual AchievementLog AchievementLog { get; set; }
        /// <summary> Relation Label: FK_AchievementConditionProgress_achievement_condition_id </summary>
        public virtual AchievementCondition AchievementCondition { get; set; }
        #endregion
    }
}