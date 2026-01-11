using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("SeasonPassMission")]
    public class SeasonPassMissionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("season_pass_id")]
        public Guid SeasonPassId { get; set; }
        [Column("mission_code")]
        public string MissionCode { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("type")]
        public SeasonPassMissionType Type { get; set; }
        [Column("group")]
        public string Group { get; set; }
        [Column("repeatable")]
        public bool Repeatable { get; set; }
        [Column("goal")]
        public int Goal { get; set; }
        [Column("reward_exp")]
        public int RewardExp { get; set; }
        [Column("order_index")]
        public int OrderIndex { get; set; }

        #region Navigation Properties
        public Guid SeasonPassDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_SeasonPassMission_season_pass_id </summary>
        public virtual SeasonPassDefinitionEntity SeasonPassDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerSeasonPassMission_season_pass_mission_id </summary>
        public virtual ICollection<PlayerSeasonPassMissionEntity> PlayerSeasonPassMissions { get; set; } = new HashSet<PlayerSeasonPassMissionEntity>();
        /// <summary> Relation Label: FK_SeasonPassMissionLog_mission_id </summary>
        public virtual ICollection<SeasonPassMissionLogEntity> SeasonPassMissionLogs { get; set; } = new HashSet<SeasonPassMissionLogEntity>();
        #endregion
    }
}