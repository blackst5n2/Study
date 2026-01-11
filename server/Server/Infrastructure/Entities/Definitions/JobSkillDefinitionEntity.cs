using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("JobSkillDefinition")]
    public class JobSkillDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("job_id")]
        public Guid JobId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("display_name")]
        public string DisplayName { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("unlock_condition")]
        public string UnlockCondition { get; set; }
        [Column("icon")]
        public string Icon { get; set; }
        [Column("max_level")]
        public int MaxLevel { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid JobDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_JobSkillDefinition_job_id </summary>
        public virtual JobDefinitionEntity JobDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerJobSkill_skill_id </summary>
        public virtual ICollection<PlayerJobSkillEntity> PlayerJobSkills { get; set; } = new HashSet<PlayerJobSkillEntity>();
        /// <summary> Relation Label: FK_JobSkillLevelDefinition_skill_id </summary>
        public virtual ICollection<JobSkillLevelDefinitionEntity> JobSkillLevelDefinitions { get; set; } = new HashSet<JobSkillLevelDefinitionEntity>();
        /// <summary> Relation Label: FK_JobSkillTree_parent_skill_id </summary>
        public virtual ICollection<JobSkillTreeEntity> JobSkillTrees { get; set; } = new HashSet<JobSkillTreeEntity>();
        #endregion
    }
}