using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("FarmPlotDefinition")]
    public class FarmPlotDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("map_id")]
        public Guid? MapId { get; set; }
        [Column("building_definition_id")]
        public Guid? BuildingDefinitionId { get; set; }
        [Column("allowed_crop_tags")]
        public string AllowedCropTags { get; set; }
        [Column("allowed_fertilizer_tags")]
        public string AllowedFertilizerTags { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MapDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_FarmPlotDefinition_map_id </summary>
        public virtual MapDefinitionEntity MapDefinition { get; set; }
        /// <summary> Relation Label: FK_FarmPlotDefinition_building_definition_id </summary>
        public virtual BuildingDefinitionEntity BuildingDefinition { get; set; }
        #endregion
    }
}