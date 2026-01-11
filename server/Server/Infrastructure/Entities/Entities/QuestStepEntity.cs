using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("QuestStep")]
    public class QuestStepEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("quest_id")]
        public Guid QuestId { get; set; }
        [Column("step_order")]
        public int StepOrder { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid QuestDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_QuestStep_quest_id </summary>
        public virtual QuestDefinitionEntity QuestDefinition { get; set; }
        #endregion
    }
}