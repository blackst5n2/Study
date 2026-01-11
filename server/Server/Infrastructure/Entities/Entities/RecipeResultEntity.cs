using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("RecipeResult")]
    public class RecipeResultEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("recipe_id")]
        public Guid RecipeId { get; set; }
        [Column("result_type")]
        public RecipeResultType ResultType { get; set; }
        [Column("item_definition_id")]
        public Guid ItemDefinitionId { get; set; }
        [Column("min_quantity")]
        public int MinQuantity { get; set; }
        [Column("max_quantity")]
        public int MaxQuantity { get; set; }
        [Column("probability")]
        public float Probability { get; set; }
        [Column("grade")]
        public ItemGrade Grade { get; set; }
        [Column("is_fail_reward")]
        public bool IsFailReward { get; set; }
        [Column("bonus_effect")]
        public string BonusEffect { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid RecipeDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_RecipeResult_recipe_id </summary>
        public virtual RecipeDefinitionEntity RecipeDefinition { get; set; }
        /// <summary> Relation Label: FK_RecipeResult_item_definition_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        #endregion
    }
}