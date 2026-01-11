using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("RecipeIngredient")]
    public class RecipeIngredientEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("recipe_id")]
        public Guid RecipeId { get; set; }
        [Column("item_definition_id")]
        public Guid ItemDefinitionId { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("consumed")]
        public bool Consumed { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid RecipeDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_RecipeIngredient_recipe_id </summary>
        public virtual RecipeDefinitionEntity RecipeDefinition { get; set; }
        /// <summary> Relation Label: FK_RecipeIngredient_item_definition_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        #endregion
    }
}