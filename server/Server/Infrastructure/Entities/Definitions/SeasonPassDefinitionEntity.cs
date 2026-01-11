using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("SeasonPassDefinition")]
    public class SeasonPassDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }
        [Column("priority")]
        public int Priority { get; set; }
        [Column("start_at")]
        public DateTime StartAt { get; set; }
        [Column("end_at")]
        public DateTime EndAt { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("free_track")]
        public bool FreeTrack { get; set; }
        [Column("premium_track")]
        public bool PremiumTrack { get; set; }
        [Column("max_level")]
        public int? MaxLevel { get; set; }
        [Column("premium_product_id")]
        public string PremiumProductId { get; set; }
        [Column("level_skip_product_id")]
        public string LevelSkipProductId { get; set; }
        [Column("config")]
        public string Config { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_SeasonPassMission_season_pass_id </summary>
        public virtual ICollection<SeasonPassMissionEntity> SeasonPassMissions { get; set; } = new HashSet<SeasonPassMissionEntity>();
        /// <summary> Relation Label: FK_PlayerSeasonPass_season_pass_id </summary>
        public virtual ICollection<PlayerSeasonPassEntity> PlayerSeasonPasses { get; set; } = new HashSet<PlayerSeasonPassEntity>();
        /// <summary> Relation Label: FK_SeasonPassReward_season_pass_id </summary>
        public virtual ICollection<SeasonPassRewardEntity> SeasonPassRewards { get; set; } = new HashSet<SeasonPassRewardEntity>();
        /// <summary> Relation Label: FK_SeasonPassPurchaseLog_season_pass_id </summary>
        public virtual ICollection<SeasonPassPurchaseLogEntity> SeasonPassPurchaseLogs { get; set; } = new HashSet<SeasonPassPurchaseLogEntity>();
        /// <summary> Relation Label: FK_SeasonPassMissionLog_season_pass_id </summary>
        public virtual ICollection<SeasonPassMissionLogEntity> SeasonPassMissionLogs { get; set; } = new HashSet<SeasonPassMissionLogEntity>();
        #endregion
    }
}