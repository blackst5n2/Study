using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("SeasonPassReward")]
    public class SeasonPassRewardEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("season_pass_id")]
        public Guid SeasonPassId { get; set; }
        [Column("reward_code")]
        public string RewardCode { get; set; }
        [Column("track")]
        public SeasonPassRewardTrack Track { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("reward_type")]
        public string RewardType { get; set; }
        [Column("is_hidden")]
        public bool IsHidden { get; set; }
        [Column("condition")]
        public string Condition { get; set; }
        [Column("mail_attachment_id")]
        public Guid? MailAttachmentId { get; set; }
        [Column("direct_reward")]
        public string DirectReward { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid SeasonPassDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_SeasonPassReward_season_pass_id </summary>
        public virtual SeasonPassDefinitionEntity SeasonPassDefinition { get; set; }
        /// <summary> Relation Label: FK_SeasonPassReward_attachment_id </summary>
        public virtual MailAttachmentEntity MailAttachment { get; set; }
        /// <summary> Relation Label: FK_PlayerSeasonPassRewardLog_season_pass_reward_id </summary>
        public virtual ICollection<PlayerSeasonPassRewardLogEntity> PlayerSeasonPassRewardLogs { get; set; } = new HashSet<PlayerSeasonPassRewardLogEntity>();
        #endregion
    }
}