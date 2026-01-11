using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class RecipeResult
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public RecipeResultType ResultType { get; set; }
        public Guid ItemDefinitionId { get; set; }
        public int MinQuantity { get; set; }
        public int MaxQuantity { get; set; }
        public float Probability { get; set; }
        public ItemGrade Grade { get; set; }
        public bool IsFailReward { get; set; }
        public string BonusEffect { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_RecipeResult_recipe_id </summary>
        public virtual RecipeDefinition RecipeDefinition { get; set; }
        /// <summary> Relation Label: FK_RecipeResult_item_definition_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        #endregion
    }
}