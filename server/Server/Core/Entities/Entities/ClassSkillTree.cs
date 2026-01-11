using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Entities
{
    public class ClassSkillTree
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public Guid? ParentSkillId { get; set; }
        public Guid ChildSkillId { get; set; }
        public string UnlockCondition { get; set; }
        public int? RequiredClassLevel { get; set; }
        public int? RequiredSkillLevel { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ClassSkillTree_class_id </summary>
        public virtual ClassDefinition ClassDefinition { get; set; }
        /// <summary> Relation Label: FK_ClassSkillTree_parent_skill_id </summary>
        public virtual SkillDefinition SkillDefinition { get; set; }
        #endregion
    }
}