using Server.Core.Entities.Entities;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class JobDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string UnlockCondition { get; set; }
        public string Icon { get; set; }
        public JobType JobType { get; set; }
        public bool IsPlayable { get; set; }
        public bool IsHidden { get; set; }
        public int OrderIndex { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_JobSkillDefinition_job_id </summary>
        public virtual ICollection<JobSkillDefinition> JobSkillDefinitions { get; set; } = new HashSet<JobSkillDefinition>();
        /// <summary> Relation Label: FK_PlayerJobSkillPoint_job_id </summary>
        public virtual ICollection<PlayerJobSkillPoint> PlayerJobSkillPoints { get; set; } = new HashSet<PlayerJobSkillPoint>();
        /// <summary> Relation Label: FK_JobLevelDefinition_job_id </summary>
        public virtual ICollection<JobLevelDefinition> JobLevelDefinitions { get; set; } = new HashSet<JobLevelDefinition>();
        /// <summary> Relation Label: FK_JobSkillTree_job_id </summary>
        public virtual ICollection<JobSkillTree> JobSkillTrees { get; set; } = new HashSet<JobSkillTree>();
        /// <summary> Relation Label: FK_JobTree_parent_job_id </summary>
        public virtual ICollection<JobTree> JobTrees { get; set; } = new HashSet<JobTree>();
        /// <summary> Relation Label: N:M via QuestJobMapping </summary>
        public virtual ICollection<QuestDefinition> QuestDefinitions { get; set; } = new HashSet<QuestDefinition>();
        /// <summary> Relation Label: N:M via AchievementJobMapping </summary>
        public virtual ICollection<AchievementDefinition> AchievementDefinitions { get; set; } = new HashSet<AchievementDefinition>();
        #endregion
    }
}