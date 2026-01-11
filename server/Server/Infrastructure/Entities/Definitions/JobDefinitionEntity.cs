using Server.Enums;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("JobDefinition")]
    public class JobDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
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
        [Column("job_type")]
        public JobType JobType { get; set; }
        [Column("is_playable")]
        public bool IsPlayable { get; set; }
        [Column("is_hidden")]
        public bool IsHidden { get; set; }
        [Column("order_index")]
        public int OrderIndex { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_JobSkillDefinition_job_id </summary>
        public virtual ICollection<JobSkillDefinitionEntity> JobSkillDefinitions { get; set; } = new HashSet<JobSkillDefinitionEntity>();
        /// <summary> Relation Label: FK_PlayerJobSkillPoint_job_id </summary>
        public virtual ICollection<PlayerJobSkillPointEntity> PlayerJobSkillPoints { get; set; } = new HashSet<PlayerJobSkillPointEntity>();
        /// <summary> Relation Label: FK_JobLevelDefinition_job_id </summary>
        public virtual ICollection<JobLevelDefinitionEntity> JobLevelDefinitions { get; set; } = new HashSet<JobLevelDefinitionEntity>();
        /// <summary> Relation Label: FK_JobSkillTree_job_id </summary>
        public virtual ICollection<JobSkillTreeEntity> JobSkillTrees { get; set; } = new HashSet<JobSkillTreeEntity>();
        /// <summary> Relation Label: FK_JobTree_parent_job_id </summary>
        public virtual ICollection<JobTreeEntity> JobTrees { get; set; } = new HashSet<JobTreeEntity>();
        /// <summary> Relation Label: N:M via QuestJobMapping </summary>
        public virtual ICollection<QuestDefinitionEntity> QuestDefinitions { get; set; } = new HashSet<QuestDefinitionEntity>();
        /// <summary> Relation Label: N:M via AchievementJobMapping </summary>
        public virtual ICollection<AchievementDefinitionEntity> AchievementDefinitions { get; set; } = new HashSet<AchievementDefinitionEntity>();
        #endregion
    }
}