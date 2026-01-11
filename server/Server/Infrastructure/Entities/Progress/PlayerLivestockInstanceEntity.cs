using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerLivestockInstance")]
    public class PlayerLivestockInstanceEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("livestock_definition_id")]
        public Guid LivestockDefinitionId { get; set; }
        [Column("nickname")]
        public string Nickname { get; set; }
        [Column("grade")]
        public ItemGrade Grade { get; set; }
        [Column("acquired_at")]
        public DateTime AcquiredAt { get; set; }
        [Column("current_growth_stage")]
        public LivestockGrowthStage CurrentGrowthStage { get; set; }
        [Column("current_growth_time_minutes")]
        public float CurrentGrowthTimeMinutes { get; set; }
        [Column("last_fed_at")]
        public DateTime? LastFedAt { get; set; }
        [Column("current_nutrition")]
        public int CurrentNutrition { get; set; }
        [Column("last_product_collected_at")]
        public DateTime? LastProductCollectedAt { get; set; }
        [Column("disease_state")]
        public string DiseaseState { get; set; }
        [Column("ranch_building_id")]
        public Guid? RanchBuildingId { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerLivestockInstance_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_PlayerLivestockInstance_livestock_definition_id </summary>
        public virtual LivestockDefinitionEntity LivestockDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerLivestockProductLog_player_livestock_instance_id </summary>
        public virtual ICollection<PlayerLivestockProductLogEntity> PlayerLivestockProductLogs { get; set; } = new HashSet<PlayerLivestockProductLogEntity>();
        /// <summary> Relation Label: FK_PlayerLivestockFeedLog_player_livestock_instance_id </summary>
        public virtual ICollection<PlayerLivestockFeedLogEntity> PlayerLivestockFeedLogs { get; set; } = new HashSet<PlayerLivestockFeedLogEntity>();
        /// <summary> Relation Label: FK_PlayerLivestockDiseaseLog_player_livestock_instance_id </summary>
        public virtual ICollection<PlayerLivestockDiseaseLogEntity> PlayerLivestockDiseaseLogs { get; set; } = new HashSet<PlayerLivestockDiseaseLogEntity>();
        /// <summary> Relation Label: FK_PlayerLivestockGradeLog_player_livestock_instance_id </summary>
        public virtual ICollection<PlayerLivestockGradeLogEntity> PlayerLivestockGradeLogs { get; set; } = new HashSet<PlayerLivestockGradeLogEntity>();
        /// <summary> Relation Label: FK_PlayerLivestockGrowthLog_player_livestock_instance_id </summary>
        public virtual ICollection<PlayerLivestockGrowthLogEntity> PlayerLivestockGrowthLogs { get; set; } = new HashSet<PlayerLivestockGrowthLogEntity>();
        /// <summary> Relation Label: FK_PlayerBuildingSlot_livestock_instance_id </summary>
        public virtual ICollection<PlayerBuildingSlotEntity> PlayerBuildingSlots { get; set; } = new HashSet<PlayerBuildingSlotEntity>();
        #endregion
    }
}