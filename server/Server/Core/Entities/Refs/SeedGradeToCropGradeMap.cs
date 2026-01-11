using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Refs
{
    public class SeedGradeToCropGradeMap
    {
        public Guid Id { get; set; }
        public Guid SeedItemId { get; set; }
        public ItemGrade SeedGrade { get; set; }
        public Guid CropDefinitionId { get; set; }
        public Guid? FertilizerItemId { get; set; }
        public string EnvironmentCondition { get; set; }
        public CropGrade CropGrade { get; set; }
        public float Probability { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_SeedGradeToCropGradeMap_seed_item_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_SeedGradeToCropGradeMap_crop_definition_id </summary>
        public virtual CropDefinition CropDefinition { get; set; }
        #endregion
    }
}