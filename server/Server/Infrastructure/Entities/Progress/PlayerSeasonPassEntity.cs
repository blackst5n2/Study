using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerSeasonPass")]
    public class PlayerSeasonPassEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("season_pass_id")]
        public Guid SeasonPassId { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("premium_unlocked")]
        public bool PremiumUnlocked { get; set; }
        [Column("current_level")]
        public int CurrentLevel { get; set; }
        [Column("current_exp")]
        public int CurrentExp { get; set; }
        [Column("last_mission_refresh_at")]
        public DateTime? LastMissionRefreshAt { get; set; }
        [Column("last_updated_at")]
        public DateTime LastUpdatedAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid SeasonPassDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerSeasonPass_season_pass_id </summary>
        public virtual SeasonPassDefinitionEntity SeasonPassDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerSeasonPass_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_PlayerSeasonPassMission_player_season_pass_id </summary>
        public virtual ICollection<PlayerSeasonPassMissionEntity> PlayerSeasonPassMissions { get; set; } = new HashSet<PlayerSeasonPassMissionEntity>();
        #endregion
    }
}