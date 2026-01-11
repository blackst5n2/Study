using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Progress
{
    public class PlayerRecipe
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid RecipeDefinitionId { get; set; }
        public DateTime UnlockedAt { get; set; }
        public bool IsKnown { get; set; }
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }
        public int PityCounter { get; set; }
        public DateTime? LastCraftedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerRecipe_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerRecipe_recipe_definition_id </summary>
        public virtual RecipeDefinition RecipeDefinition { get; set; }
        #endregion
    }
}