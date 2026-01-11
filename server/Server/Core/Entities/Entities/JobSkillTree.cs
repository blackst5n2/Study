using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Entities
{
    public class JobSkillTree
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public Guid? ParentSkillId { get; set; }
        public Guid ChildSkillId { get; set; }
        public string UnlockCondition { get; set; }
        public int? RequiredJobLevel { get; set; }
        public int? RequiredSkillLevel { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_JobSkillTree_job_id </summary>
        public virtual JobDefinition JobDefinition { get; set; }
        /// <summary> Relation Label: FK_JobSkillTree_parent_skill_id </summary>
        public virtual JobSkillDefinition JobSkillDefinition { get; set; }
        #endregion
    }
}