using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Entities
{
    public class JobTree
    {
        public Guid Id { get; set; }
        public Guid? ParentJobId { get; set; }
        public Guid ChildJobId { get; set; }
        public string UnlockCondition { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_JobTree_parent_job_id </summary>
        public virtual JobDefinition JobDefinition { get; set; }
        #endregion
    }
}