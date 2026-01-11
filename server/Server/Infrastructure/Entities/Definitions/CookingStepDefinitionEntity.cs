using Server.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("CookingStepDefinition")]
    public class CookingStepDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("recipe_id")]
        public Guid RecipeId { get; set; }
        [Column("step_index")]
        public int StepIndex { get; set; }
        [Column("action")]
        public CookingAction Action { get; set; }
        [Column("required_tool")]
        public ItemSubType RequiredTool { get; set; }
        [Column("required_time_sec")]
        public int? RequiredTimeSec { get; set; }
        [Column("temperature")]
        public int? Temperature { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid CookingRecipeDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_CookingStepDefinition_recipe_id </summary>
        public virtual CookingRecipeDefinitionEntity CookingRecipeDefinition { get; set; }
        #endregion
    }
}