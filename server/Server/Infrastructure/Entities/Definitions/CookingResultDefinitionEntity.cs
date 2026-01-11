using Server.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("CookingResultDefinition")]
    public class CookingResultDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("recipe_id")]
        public Guid RecipeId { get; set; }
        [Column("doneness")]
        public Doneness Doneness { get; set; }
        [Column("temperature_range")]
        public string TemperatureRange { get; set; }
        [Column("result_item_id")]
        public Guid ResultItemId { get; set; }
        [Column("result_quantity")]
        public int ResultQuantity { get; set; }
        [Column("probability")]
        public float Probability { get; set; }
        [Column("bonus_effect")]
        public string BonusEffect { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid CookingRecipeDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_CookingResultDefinition_recipe_id </summary>
        public virtual CookingRecipeDefinitionEntity CookingRecipeDefinition { get; set; }
        public Guid ItemDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_CookingResultDefinition_result_item_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        #endregion
    }
}