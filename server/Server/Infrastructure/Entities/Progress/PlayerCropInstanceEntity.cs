using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerCropInstance")]
    public class PlayerCropInstanceEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("crop_definition_id")]
        public Guid CropDefinitionId { get; set; }
        [Column("farm_plot_id")]
        public Guid? FarmPlotId { get; set; }
        [Column("land_plot_id")]
        public Guid? LandPlotId { get; set; }
        [Column("pos_x")]
        public float PosX { get; set; }
        [Column("pos_y")]
        public float PosY { get; set; }
        [Column("pos_z")]
        public float PosZ { get; set; }
        [Column("plant_time")]
        public DateTime PlantTime { get; set; }
        [Column("growth_stage")]
        public int GrowthStage { get; set; }
        [Column("current_growth_time_minutes")]
        public float CurrentGrowthTimeMinutes { get; set; }
        [Column("is_watered")]
        public bool IsWatered { get; set; }
        [Column("last_watered_at")]
        public DateTime? LastWateredAt { get; set; }
        [Column("disease_state")]
        public string DiseaseState { get; set; }
        [Column("expected_harvest_time")]
        public DateTime? ExpectedHarvestTime { get; set; }
        [Column("actual_harvest_time")]
        public DateTime? ActualHarvestTime { get; set; }
        [Column("applied_fertilizer")]
        public string AppliedFertilizer { get; set; }
        [Column("current_grade")]
        public CropGrade CurrentGrade { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerCropInstance_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_PlayerCropInstance_crop_definition_id </summary>
        public virtual CropDefinitionEntity CropDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerCropHarvestLog_player_crop_instance_id </summary>
        public virtual ICollection<PlayerCropHarvestLogEntity> PlayerCropHarvestLogs { get; set; } = new HashSet<PlayerCropHarvestLogEntity>();
        /// <summary> Relation Label: FK_PlayerBuildingSlot_crop_instance_id </summary>
        public virtual ICollection<PlayerBuildingSlotEntity> PlayerBuildingSlots { get; set; } = new HashSet<PlayerBuildingSlotEntity>();
        #endregion
    }
}