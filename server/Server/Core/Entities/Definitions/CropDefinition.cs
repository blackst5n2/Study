using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;
using Server.Core.Entities.Refs;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class CropDefinition
    {
        public Guid Id { get; set; }
        public Guid EntityDefinitionId { get; set; }
        public int RequiredFarmingLevel { get; set; }
        public SeasonType AllowedSeason { get; set; }
        public string FertilizerEffect { get; set; }
        public CropGrade BaseGrade { get; set; }
        public int GrowthStages { get; set; }
        public int GrowthTimeMinutesPerStage { get; set; }
        public float DiseaseChance { get; set; }
        public int RequiredNutrient { get; set; }
        public int RequiredMoisture { get; set; }
        public int? RequiredTemperature { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_CropDefinition_entity_definition_id </summary>
        public virtual EntityDefinition EntityDefinition { get; set; }
        /// <summary> Relation Label: FK_SeedGradeToCropGradeMap_crop_definition_id </summary>
        public virtual ICollection<SeedGradeToCropGradeMap> SeedGradeToCropGradeMaps { get; set; } = new HashSet<SeedGradeToCropGradeMap>();
        /// <summary> Relation Label: FK_CropProduct_crop_definition_id </summary>
        public virtual ICollection<CropProduct> CropProducts { get; set; } = new HashSet<CropProduct>();
        /// <summary> Relation Label: FK_PlayerCropInstance_crop_definition_id </summary>
        public virtual ICollection<PlayerCropInstance> PlayerCropInstances { get; set; } = new HashSet<PlayerCropInstance>();
        /// <summary> Relation Label: FK_PlayerCropHarvestLog_crop_definition_id </summary>
        public virtual ICollection<PlayerCropHarvestLog> PlayerCropHarvestLogs { get; set; } = new HashSet<PlayerCropHarvestLog>();
        #endregion
    }
}