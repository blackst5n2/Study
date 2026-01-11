using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("AchievementReward")]
    public class AchievementRewardEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("achievement_id")]
        public Guid AchievementId { get; set; }
        [Column("type")]
        public AchievementRewardType Type { get; set; }
        [Column("target_id")]
        public string TargetId { get; set; }
        [Column("value")]
        public long Value { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid AchievementDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_AchievementReward_achievement_id </summary>
        public virtual AchievementDefinitionEntity AchievementDefinition { get; set; }
        /// <summary> Relation Label: FK_AchievementRewardLog_achievement_reward_id </summary>
        public virtual ICollection<AchievementRewardLogEntity> AchievementRewardLogs { get; set; } = new HashSet<AchievementRewardLogEntity>();
        #endregion
    }
}