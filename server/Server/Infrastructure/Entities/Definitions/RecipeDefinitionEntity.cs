using Server.Enums;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("RecipeDefinition")]
    public class RecipeDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("type")]
        public RecipeType Type { get; set; }
        [Column("required_building_code")]
        public string RequiredBuildingCode { get; set; }
        [Column("required_level")]
        public int RequiredLevel { get; set; }
        [Column("success_rate")]
        public float SuccessRate { get; set; }
        [Column("craft_time_seconds")]
        public int CraftTimeSeconds { get; set; }
        [Column("unlock_condition")]
        public string UnlockCondition { get; set; }
        [Column("is_repeatable")]
        public bool IsRepeatable { get; set; }
        [Column("pity_threshold")]
        public int? PityThreshold { get; set; }
        [Column("pity_success_rate")]
        public float? PitySuccessRate { get; set; }
        [Column("fail_reward_item_id")]
        public Guid? FailRewardItemId { get; set; }
        [Column("fail_reward_quantity")]
        public int? FailRewardQuantity { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_RecipeIngredient_recipe_id </summary>
        public virtual ICollection<RecipeIngredientEntity> RecipeIngredients { get; set; } = new HashSet<RecipeIngredientEntity>();
        /// <summary> Relation Label: FK_RecipeResult_recipe_id </summary>
        public virtual ICollection<RecipeResultEntity> RecipeResults { get; set; } = new HashSet<RecipeResultEntity>();
        /// <summary> Relation Label: FK_PlayerCraftingLog_recipe_id </summary>
        public virtual ICollection<PlayerCraftingLogEntity> PlayerCraftingLogs { get; set; } = new HashSet<PlayerCraftingLogEntity>();
        /// <summary> Relation Label: FK_PlayerRecipe_recipe_definition_id </summary>
        public virtual ICollection<PlayerRecipeEntity> PlayerRecipes { get; set; } = new HashSet<PlayerRecipeEntity>();
        public Guid ItemDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_RecipeDefinition_fail_reward_item_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_CookingRecipeDefinition_recipe_definition_id </summary>
        public virtual ICollection<CookingRecipeDefinitionEntity> CookingRecipeDefinitions { get; set; } = new HashSet<CookingRecipeDefinitionEntity>();
        #endregion
    }
}