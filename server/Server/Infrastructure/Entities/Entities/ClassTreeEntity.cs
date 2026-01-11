using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("ClassTree")]
    public class ClassTreeEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("parent_class_id")]
        public Guid? ParentClassId { get; set; }
        [Column("child_class_id")]
        public Guid ChildClassId { get; set; }
        [Column("unlock_condition")]
        public string UnlockCondition { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid ClassDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_ClassTree_parent_class_id </summary>
        public virtual ClassDefinitionEntity ClassDefinition { get; set; }
        #endregion
    }
}