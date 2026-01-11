using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("ClassSkillDefinition")]
    public class ClassSkillDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("class_id")]
        public Guid ClassId { get; set; }
        [Column("skill_definition_id")]
        public Guid SkillDefinitionId { get; set; }
        [Column("unlock_level")]
        public int? UnlockLevel { get; set; }
        [Column("unlock_condition")]
        public string UnlockCondition { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid ClassDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_ClassSkillDefinition_class_id </summary>
        public virtual ClassDefinitionEntity ClassDefinition { get; set; }
        /// <summary> Relation Label: FK_ClassSkillDefinition_skill_definition_id </summary>
        public virtual SkillDefinitionEntity SkillDefinition { get; set; }
        #endregion
    }
}