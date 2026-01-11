
namespace Server.Core.Entities.Definitions
{
    public class FarmPlotDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid? MapId { get; set; }
        public Guid? BuildingDefinitionId { get; set; }
        public string AllowedCropTags { get; set; }
        public string AllowedFertilizerTags { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_FarmPlotDefinition_map_id </summary>
        public virtual MapDefinition MapDefinition { get; set; }
        /// <summary> Relation Label: FK_FarmPlotDefinition_building_definition_id </summary>
        public virtual BuildingDefinition BuildingDefinition { get; set; }
        #endregion
    }
}