using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Entities
{
    public class RecipeIngredient
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public Guid ItemDefinitionId { get; set; }
        public int Quantity { get; set; }
        public bool Consumed { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_RecipeIngredient_recipe_id </summary>
        public virtual RecipeDefinition RecipeDefinition { get; set; }
        /// <summary> Relation Label: FK_RecipeIngredient_item_definition_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        #endregion
    }
}