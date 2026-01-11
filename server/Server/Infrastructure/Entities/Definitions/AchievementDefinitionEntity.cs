using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("AchievementDefinition")]
    public class AchievementDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("category_id")]
        public Guid? CategoryId { get; set; }
        [Column("required_level")]
        public int RequiredLevel { get; set; }
        [Column("points")]
        public int Points { get; set; }
        [Column("is_hidden")]
        public bool IsHidden { get; set; }
        [Column("icon")]
        public string Icon { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid AchievementCategoryId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_AchievementDefinition_category_id </summary>
        public virtual AchievementCategoryEntity AchievementCategory { get; set; }
        /// <summary> Relation Label: FK_AchievementCondition_achievement_id </summary>
        public virtual ICollection<AchievementConditionEntity> AchievementConditions { get; set; } = new HashSet<AchievementConditionEntity>();
        /// <summary> Relation Label: FK_AchievementReward_achievement_id </summary>
        public virtual ICollection<AchievementRewardEntity> AchievementRewards { get; set; } = new HashSet<AchievementRewardEntity>();
        /// <summary> Relation Label: FK_AchievementLog_achievement_id </summary>
        public virtual ICollection<AchievementLogEntity> AchievementLogs { get; set; } = new HashSet<AchievementLogEntity>();
        /// <summary> Relation Label: N:M via AchievementClassMapping </summary>
        public virtual ICollection<ClassDefinitionEntity> ClassDefinitions { get; set; } = new HashSet<ClassDefinitionEntity>();
        /// <summary> Relation Label: N:M via AchievementJobMapping </summary>
        public virtual ICollection<JobDefinitionEntity> JobDefinitions { get; set; } = new HashSet<JobDefinitionEntity>();
        #endregion
    }
}