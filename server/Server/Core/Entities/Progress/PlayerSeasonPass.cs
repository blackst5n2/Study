using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Progress
{
    public class PlayerSeasonPass
    {
        public Guid Id { get; set; }
        public Guid SeasonPassId { get; set; }
        public Guid PlayerId { get; set; }
        public bool PremiumUnlocked { get; set; }
        public int CurrentLevel { get; set; }
        public int CurrentExp { get; set; }
        public DateTime? LastMissionRefreshAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerSeasonPass_season_pass_id </summary>
        public virtual SeasonPassDefinition SeasonPassDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerSeasonPass_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerSeasonPassMission_player_season_pass_id </summary>
        public virtual ICollection<PlayerSeasonPassMission> PlayerSeasonPassMissions { get; set; } = new HashSet<PlayerSeasonPassMission>();
        #endregion
    }
}