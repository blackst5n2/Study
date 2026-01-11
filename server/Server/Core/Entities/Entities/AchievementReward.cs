using Server.Core.Entities.Definitions;
using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class AchievementReward
    {
        public Guid Id { get; set; }
        public Guid AchievementId { get; set; }
        public AchievementRewardType Type { get; set; }
        public string TargetId { get; set; }
        public long Value { get; set; }
        public string Description { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_AchievementReward_achievement_id </summary>
        public virtual AchievementDefinition AchievementDefinition { get; set; }
        /// <summary> Relation Label: FK_AchievementRewardLog_achievement_reward_id </summary>
        public virtual ICollection<AchievementRewardLog> AchievementRewardLogs { get; set; } = new HashSet<AchievementRewardLog>();
        #endregion
    }
}