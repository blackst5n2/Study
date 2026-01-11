using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Definitions
{
    public class SeasonPassDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Priority { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public bool IsActive { get; set; }
        public bool FreeTrack { get; set; }
        public bool PremiumTrack { get; set; }
        public int? MaxLevel { get; set; }
        public string PremiumProductId { get; set; }
        public string LevelSkipProductId { get; set; }
        public string Config { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_SeasonPassMission_season_pass_id </summary>
        public virtual ICollection<SeasonPassMission> SeasonPassMissions { get; set; } = new HashSet<SeasonPassMission>();
        /// <summary> Relation Label: FK_PlayerSeasonPass_season_pass_id </summary>
        public virtual ICollection<PlayerSeasonPass> PlayerSeasonPasses { get; set; } = new HashSet<PlayerSeasonPass>();
        /// <summary> Relation Label: FK_SeasonPassReward_season_pass_id </summary>
        public virtual ICollection<SeasonPassReward> SeasonPassRewards { get; set; } = new HashSet<SeasonPassReward>();
        /// <summary> Relation Label: FK_SeasonPassPurchaseLog_season_pass_id </summary>
        public virtual ICollection<SeasonPassPurchaseLog> SeasonPassPurchaseLogs { get; set; } = new HashSet<SeasonPassPurchaseLog>();
        /// <summary> Relation Label: FK_SeasonPassMissionLog_season_pass_id </summary>
        public virtual ICollection<SeasonPassMissionLog> SeasonPassMissionLogs { get; set; } = new HashSet<SeasonPassMissionLog>();
        #endregion
    }
}