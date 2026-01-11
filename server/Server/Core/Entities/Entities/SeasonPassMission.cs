using Server.Core.Entities.Definitions;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class SeasonPassMission
    {
        public Guid Id { get; set; }
        public Guid SeasonPassId { get; set; }
        public string MissionCode { get; set; }
        public string Description { get; set; }
        public SeasonPassMissionType Type { get; set; }
        public string Group { get; set; }
        public bool Repeatable { get; set; }
        public int Goal { get; set; }
        public int RewardExp { get; set; }
        public int OrderIndex { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_SeasonPassMission_season_pass_id </summary>
        public virtual SeasonPassDefinition SeasonPassDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerSeasonPassMission_season_pass_mission_id </summary>
        public virtual ICollection<PlayerSeasonPassMission> PlayerSeasonPassMissions { get; set; } = new HashSet<PlayerSeasonPassMission>();
        /// <summary> Relation Label: FK_SeasonPassMissionLog_mission_id </summary>
        public virtual ICollection<SeasonPassMissionLog> SeasonPassMissionLogs { get; set; } = new HashSet<SeasonPassMissionLog>();
        #endregion
    }
}