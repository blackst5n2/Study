using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("ClassSkillTree")]
    public class ClassSkillTreeEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("class_id")]
        public Guid ClassId { get; set; }
        [Column("parent_skill_id")]
        public Guid? ParentSkillId { get; set; }
        [Column("child_skill_id")]
        public Guid ChildSkillId { get; set; }
        [Column("unlock_condition")]
        public string UnlockCondition { get; set; }
        [Column("required_class_level")]
        public int? RequiredClassLevel { get; set; }
        [Column("required_skill_level")]
        public int? RequiredSkillLevel { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid ClassDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_ClassSkillTree_class_id </summary>
        public virtual ClassDefinitionEntity ClassDefinition { get; set; }
        public Guid SkillDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_ClassSkillTree_parent_skill_id </summary>
        public virtual SkillDefinitionEntity SkillDefinition { get; set; }
        #endregion
    }
}