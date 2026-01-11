using Server.Core.Entities.Entities;

namespace Server.Core.Entities.Progress
{
    public class PlayerSeasonPassMission
    {
        public Guid Id { get; set; }
        public Guid PlayerSeasonPassId { get; set; }
        public Guid SeasonPassMissionId { get; set; }
        public int Progress { get; set; }
        public DateTime? LastProgressAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? ClaimedAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerSeasonPassMission_player_season_pass_id </summary>
        public virtual PlayerSeasonPass PlayerSeasonPass { get; set; }
        /// <summary> Relation Label: FK_PlayerSeasonPassMission_season_pass_mission_id </summary>
        public virtual SeasonPassMission SeasonPassMission { get; set; }
        #endregion
    }
}