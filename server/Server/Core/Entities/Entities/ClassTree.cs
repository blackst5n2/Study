using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Entities
{
    public class ClassTree
    {
        public Guid Id { get; set; }
        public Guid? ParentClassId { get; set; }
        public Guid ChildClassId { get; set; }
        public string UnlockCondition { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ClassTree_parent_class_id </summary>
        public virtual ClassDefinition ClassDefinition { get; set; }
        #endregion
    }
}