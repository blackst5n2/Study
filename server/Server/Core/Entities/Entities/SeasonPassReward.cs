using Server.Core.Entities.Definitions;
using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class SeasonPassReward
    {
        public Guid Id { get; set; }
        public Guid SeasonPassId { get; set; }
        public string RewardCode { get; set; }
        public SeasonPassRewardTrack Track { get; set; }
        public int Level { get; set; }
        public string RewardType { get; set; }
        public bool IsHidden { get; set; }
        public string Condition { get; set; }
        public Guid? MailAttachmentId { get; set; }
        public string DirectReward { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_SeasonPassReward_season_pass_id </summary>
        public virtual SeasonPassDefinition SeasonPassDefinition { get; set; }
        /// <summary> Relation Label: FK_SeasonPassReward_attachment_id </summary>
        public virtual MailAttachment MailAttachment { get; set; }
        /// <summary> Relation Label: FK_PlayerSeasonPassRewardLog_season_pass_reward_id </summary>
        public virtual ICollection<PlayerSeasonPassRewardLog> PlayerSeasonPassRewardLogs { get; set; } = new HashSet<PlayerSeasonPassRewardLog>();
        #endregion
    }
}