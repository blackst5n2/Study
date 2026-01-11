using Server.Enums;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using Server.Infrastructure.Entities.Refs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("CropDefinition")]
    public class CropDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("entity_definition_id")]
        public Guid EntityDefinitionId { get; set; }
        [Column("required_farming_level")]
        public int RequiredFarmingLevel { get; set; }
        [Column("allowed_season")]
        public SeasonType AllowedSeason { get; set; }
        [Column("fertilizer_effect")]
        public string FertilizerEffect { get; set; }
        [Column("base_grade")]
        public CropGrade BaseGrade { get; set; }
        [Column("growth_stages")]
        public int GrowthStages { get; set; }
        [Column("growth_time_minutes_per_stage")]
        public int GrowthTimeMinutesPerStage { get; set; }
        [Column("disease_chance")]
        public float DiseaseChance { get; set; }
        [Column("required_nutrient")]
        public int RequiredNutrient { get; set; }
        [Column("required_moisture")]
        public int RequiredMoisture { get; set; }
        [Column("required_temperature")]
        public int? RequiredTemperature { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_CropDefinition_entity_definition_id </summary>
        public virtual EntityDefinitionEntity EntityDefinition { get; set; }
        /// <summary> Relation Label: FK_SeedGradeToCropGradeMap_crop_definition_id </summary>
        public virtual ICollection<SeedGradeToCropGradeMapEntity> SeedGradeToCropGradeMaps { get; set; } = new HashSet<SeedGradeToCropGradeMapEntity>();
        /// <summary> Relation Label: FK_CropProduct_crop_definition_id </summary>
        public virtual ICollection<CropProductEntity> CropProducts { get; set; } = new HashSet<CropProductEntity>();
        /// <summary> Relation Label: FK_PlayerCropInstance_crop_definition_id </summary>
        public virtual ICollection<PlayerCropInstanceEntity> PlayerCropInstances { get; set; } = new HashSet<PlayerCropInstanceEntity>();
        /// <summary> Relation Label: FK_PlayerCropHarvestLog_crop_definition_id </summary>
        public virtual ICollection<PlayerCropHarvestLogEntity> PlayerCropHarvestLogs { get; set; } = new HashSet<PlayerCropHarvestLogEntity>();
        #endregion
    }
}