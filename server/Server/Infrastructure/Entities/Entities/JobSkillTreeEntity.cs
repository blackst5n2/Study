using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("JobSkillTree")]
    public class JobSkillTreeEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("job_id")]
        public Guid JobId { get; set; }
        [Column("parent_skill_id")]
        public Guid? ParentSkillId { get; set; }
        [Column("child_skill_id")]
        public Guid ChildSkillId { get; set; }
        [Column("unlock_condition")]
        public string UnlockCondition { get; set; }
        [Column("required_job_level")]
        public int? RequiredJobLevel { get; set; }
        [Column("required_skill_level")]
        public int? RequiredSkillLevel { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid JobDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_JobSkillTree_job_id </summary>
        public virtual JobDefinitionEntity JobDefinition { get; set; }
        public Guid JobSkillDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_JobSkillTree_parent_skill_id </summary>
        public virtual JobSkillDefinitionEntity JobSkillDefinition { get; set; }
        #endregion
    }
}