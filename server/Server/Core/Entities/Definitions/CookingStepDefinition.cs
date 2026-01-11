using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class CookingStepDefinition
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public int StepIndex { get; set; }
        public CookingAction Action { get; set; }
        public ItemSubType RequiredTool { get; set; }
        public int? RequiredTimeSec { get; set; }
        public int? Temperature { get; set; }
        public string Description { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_CookingStepDefinition_recipe_id </summary>
        public virtual CookingRecipeDefinition CookingRecipeDefinition { get; set; }
        #endregion
    }
}