using Server.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("CookingGradeRewardDefinition")]
    public class CookingGradeRewardDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("recipe_id")]
        public Guid RecipeId { get; set; }
        [Column("grade")]
        public CookingGrade Grade { get; set; }
        [Column("reward_item_id")]
        public Guid RewardItemId { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("probability")]
        public float Probability { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid CookingRecipeDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_CookingGradeRewardDefinition_recipe_id </summary>
        public virtual CookingRecipeDefinitionEntity CookingRecipeDefinition { get; set; }
        public Guid ItemDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_CookingGradeRewardDefinition_reward_item_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        #endregion
    }
}