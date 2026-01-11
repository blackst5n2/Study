using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class CookingRecipeDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid RecipeDefinitionId { get; set; }
        public string TemperatureRange { get; set; }
        public Doneness OptimalDoneness { get; set; }
        public string BonusEffect { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_CookingRecipeDefinition_recipe_definition_id </summary>
        public virtual RecipeDefinition RecipeDefinition { get; set; }
        /// <summary> Relation Label: FK_CookingResultDefinition_recipe_id </summary>
        public virtual ICollection<CookingResultDefinition> CookingResultDefinitions { get; set; } = new HashSet<CookingResultDefinition>();
        /// <summary> Relation Label: FK_CookingGradeRewardDefinition_recipe_id </summary>
        public virtual ICollection<CookingGradeRewardDefinition> CookingGradeRewardDefinitions { get; set; } = new HashSet<CookingGradeRewardDefinition>();
        /// <summary> Relation Label: FK_CookingStepDefinition_recipe_id </summary>
        public virtual ICollection<CookingStepDefinition> CookingStepDefinitions { get; set; } = new HashSet<CookingStepDefinition>();
        /// <summary> Relation Label: FK_PlayerCookingLog_cooking_recipe_id </summary>
        public virtual ICollection<PlayerCookingLog> PlayerCookingLogs { get; set; } = new HashSet<PlayerCookingLog>();
        #endregion
    }
}