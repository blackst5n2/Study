using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class AchievementCondition
    {
        public Guid Id { get; set; }
        public Guid AchievementId { get; set; }
        public int Sequence { get; set; }
        public AchievementConditionType Type { get; set; }
        public string TargetId { get; set; }
        public long RequiredValue { get; set; }
        public string Description { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_AchievementCondition_achievement_id </summary>
        public virtual AchievementDefinition AchievementDefinition { get; set; }
        /// <summary> Relation Label: FK_AchievementConditionProgress_achievement_condition_id </summary>
        public virtual ICollection<AchievementConditionProgress> AchievementConditionProgresses { get; set; } = new HashSet<AchievementConditionProgress>();
        #endregion
    }
}