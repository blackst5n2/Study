using Server.Infrastructure.Entities.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerSeasonPassMission")]
    public class PlayerSeasonPassMissionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_season_pass_id")]
        public Guid PlayerSeasonPassId { get; set; }
        [Column("season_pass_mission_id")]
        public Guid SeasonPassMissionId { get; set; }
        [Column("progress")]
        public int Progress { get; set; }
        [Column("last_progress_at")]
        public DateTime? LastProgressAt { get; set; }
        [Column("completed_at")]
        public DateTime? CompletedAt { get; set; }
        [Column("claimed_at")]
        public DateTime? ClaimedAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerSeasonPassMission_player_season_pass_id </summary>
        public virtual PlayerSeasonPassEntity PlayerSeasonPass { get; set; }
        /// <summary> Relation Label: FK_PlayerSeasonPassMission_season_pass_mission_id </summary>
        public virtual SeasonPassMissionEntity SeasonPassMission { get; set; }
        #endregion
    }
}