using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class CookingGradeRewardDefinition
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public CookingGrade Grade { get; set; }
        public Guid RewardItemId { get; set; }
        public int Quantity { get; set; }
        public float Probability { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_CookingGradeRewardDefinition_recipe_id </summary>
        public virtual CookingRecipeDefinition CookingRecipeDefinition { get; set; }
        /// <summary> Relation Label: FK_CookingGradeRewardDefinition_reward_item_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        #endregion
    }
}