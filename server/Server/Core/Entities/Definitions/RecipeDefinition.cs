using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class RecipeDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RecipeType Type { get; set; }
        public string RequiredBuildingCode { get; set; }
        public int RequiredLevel { get; set; }
        public float SuccessRate { get; set; }
        public int CraftTimeSeconds { get; set; }
        public string UnlockCondition { get; set; }
        public bool IsRepeatable { get; set; }
        public int? PityThreshold { get; set; }
        public float? PitySuccessRate { get; set; }
        public Guid? FailRewardItemId { get; set; }
        public int? FailRewardQuantity { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_RecipeIngredient_recipe_id </summary>
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new HashSet<RecipeIngredient>();
        /// <summary> Relation Label: FK_RecipeResult_recipe_id </summary>
        public virtual ICollection<RecipeResult> RecipeResults { get; set; } = new HashSet<RecipeResult>();
        /// <summary> Relation Label: FK_PlayerCraftingLog_recipe_id </summary>
        public virtual ICollection<PlayerCraftingLog> PlayerCraftingLogs { get; set; } = new HashSet<PlayerCraftingLog>();
        /// <summary> Relation Label: FK_PlayerRecipe_recipe_definition_id </summary>
        public virtual ICollection<PlayerRecipe> PlayerRecipes { get; set; } = new HashSet<PlayerRecipe>();
        /// <summary> Relation Label: FK_RecipeDefinition_fail_reward_item_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_CookingRecipeDefinition_recipe_definition_id </summary>
        public virtual ICollection<CookingRecipeDefinition> CookingRecipeDefinitions { get; set; } = new HashSet<CookingRecipeDefinition>();
        #endregion
    }
}