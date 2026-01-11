using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("JobTree")]
    public class JobTreeEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("parent_job_id")]
        public Guid? ParentJobId { get; set; }
        [Column("child_job_id")]
        public Guid ChildJobId { get; set; }
        [Column("unlock_condition")]
        public string UnlockCondition { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid JobDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_JobTree_parent_job_id </summary>
        public virtual JobDefinitionEntity JobDefinition { get; set; }
        #endregion
    }
}