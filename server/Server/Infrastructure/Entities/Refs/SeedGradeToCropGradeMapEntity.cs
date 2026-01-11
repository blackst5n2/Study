using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Refs
{
    [Table("SeedGradeToCropGradeMap")]
    public class SeedGradeToCropGradeMapEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("seed_item_id")]
        public Guid SeedItemId { get; set; }
        [Column("seed_grade")]
        public ItemGrade SeedGrade { get; set; }
        [Column("crop_definition_id")]
        public Guid CropDefinitionId { get; set; }
        [Column("fertilizer_item_id")]
        public Guid? FertilizerItemId { get; set; }
        [Column("environment_condition")]
        public string EnvironmentCondition { get; set; }
        [Column("crop_grade")]
        public CropGrade CropGrade { get; set; }
        [Column("probability")]
        public float Probability { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid ItemDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_SeedGradeToCropGradeMap_seed_item_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_SeedGradeToCropGradeMap_crop_definition_id </summary>
        public virtual CropDefinitionEntity CropDefinition { get; set; }
        #endregion
    }
}