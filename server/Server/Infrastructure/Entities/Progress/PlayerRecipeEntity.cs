using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerRecipe")]
    public class PlayerRecipeEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("recipe_definition_id")]
        public Guid RecipeDefinitionId { get; set; }
        [Column("unlocked_at")]
        public DateTime UnlockedAt { get; set; }
        [Column("is_known")]
        public bool IsKnown { get; set; }
        [Column("success_count")]
        public int SuccessCount { get; set; }
        [Column("fail_count")]
        public int FailCount { get; set; }
        [Column("pity_counter")]
        public int PityCounter { get; set; }
        [Column("last_crafted_at")]
        public DateTime? LastCraftedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerRecipe_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_PlayerRecipe_recipe_definition_id </summary>
        public virtual RecipeDefinitionEntity RecipeDefinition { get; set; }
        #endregion
    }
}