using Server.Core.Entities.Entities;
using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Definitions
{
    public class JobSkillDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid JobId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string UnlockCondition { get; set; }
        public string Icon { get; set; }
        public int MaxLevel { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_JobSkillDefinition_job_id </summary>
        public virtual JobDefinition JobDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerJobSkill_skill_id </summary>
        public virtual ICollection<PlayerJobSkill> PlayerJobSkills { get; set; } = new HashSet<PlayerJobSkill>();
        /// <summary> Relation Label: FK_JobSkillLevelDefinition_skill_id </summary>
        public virtual ICollection<JobSkillLevelDefinition> JobSkillLevelDefinitions { get; set; } = new HashSet<JobSkillLevelDefinition>();
        /// <summary> Relation Label: FK_JobSkillTree_parent_skill_id </summary>
        public virtual ICollection<JobSkillTree> JobSkillTrees { get; set; } = new HashSet<JobSkillTree>();
        #endregion
    }
}