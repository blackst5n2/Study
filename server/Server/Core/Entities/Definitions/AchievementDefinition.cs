using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;

namespace Server.Core.Entities.Definitions
{
    public class AchievementDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? CategoryId { get; set; }
        public int RequiredLevel { get; set; }
        public int Points { get; set; }
        public bool IsHidden { get; set; }
        public string Icon { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_AchievementDefinition_category_id </summary>
        public virtual AchievementCategory AchievementCategory { get; set; }
        /// <summary> Relation Label: FK_AchievementCondition_achievement_id </summary>
        public virtual ICollection<AchievementCondition> AchievementConditions { get; set; } = new HashSet<AchievementCondition>();
        /// <summary> Relation Label: FK_AchievementReward_achievement_id </summary>
        public virtual ICollection<AchievementReward> AchievementRewards { get; set; } = new HashSet<AchievementReward>();
        /// <summary> Relation Label: FK_AchievementLog_achievement_id </summary>
        public virtual ICollection<AchievementLog> AchievementLogs { get; set; } = new HashSet<AchievementLog>();
        /// <summary> Relation Label: N:M via AchievementClassMapping </summary>
        public virtual ICollection<ClassDefinition> ClassDefinitions { get; set; } = new HashSet<ClassDefinition>();
        /// <summary> Relation Label: N:M via AchievementJobMapping </summary>
        public virtual ICollection<JobDefinition> JobDefinitions { get; set; } = new HashSet<JobDefinition>();
        #endregion
    }
}