using Server.Enums;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("CookingRecipeDefinition")]
    public class CookingRecipeDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("recipe_definition_id")]
        public Guid RecipeDefinitionId { get; set; }
        [Column("temperature_range")]
        public string TemperatureRange { get; set; }
        [Column("optimal_doneness")]
        public Doneness OptimalDoneness { get; set; }
        [Column("bonus_effect")]
        public string BonusEffect { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_CookingRecipeDefinition_recipe_definition_id </summary>
        public virtual RecipeDefinitionEntity RecipeDefinition { get; set; }
        /// <summary> Relation Label: FK_CookingResultDefinition_recipe_id </summary>
        public virtual ICollection<CookingResultDefinitionEntity> CookingResultDefinitions { get; set; } = new HashSet<CookingResultDefinitionEntity>();
        /// <summary> Relation Label: FK_CookingGradeRewardDefinition_recipe_id </summary>
        public virtual ICollection<CookingGradeRewardDefinitionEntity> CookingGradeRewardDefinitions { get; set; } = new HashSet<CookingGradeRewardDefinitionEntity>();
        /// <summary> Relation Label: FK_CookingStepDefinition_recipe_id </summary>
        public virtual ICollection<CookingStepDefinitionEntity> CookingStepDefinitions { get; set; } = new HashSet<CookingStepDefinitionEntity>();
        /// <summary> Relation Label: FK_PlayerCookingLog_cooking_recipe_id </summary>
        public virtual ICollection<PlayerCookingLogEntity> PlayerCookingLogs { get; set; } = new HashSet<PlayerCookingLogEntity>();
        #endregion
    }
}