
namespace Server.Core.Entities.Definitions
{
    public class ClassSkillDefinition
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public Guid SkillDefinitionId { get; set; }
        public int? UnlockLevel { get; set; }
        public string UnlockCondition { get; set; }
        public bool IsActive { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ClassSkillDefinition_class_id </summary>
        public virtual ClassDefinition ClassDefinition { get; set; }
        /// <summary> Relation Label: FK_ClassSkillDefinition_skill_definition_id </summary>
        public virtual SkillDefinition SkillDefinition { get; set; }
        #endregion
    }
}