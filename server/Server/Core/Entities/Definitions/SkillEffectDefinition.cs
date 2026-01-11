using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class SkillEffectDefinition
    {
        public Guid Id { get; set; }
        public Guid SkillDefinitionId { get; set; }
        public int Level { get; set; }
        public SkillEffectType EffectType { get; set; }
        public string ValueFormula { get; set; }
        public float? DurationSeconds { get; set; }
        public SkillTargetType TargetType { get; set; }
        public string Condition { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_SkillEffectDefinition_skill_definition_id </summary>
        public virtual SkillDefinition SkillDefinition { get; set; }
        #endregion
    }
}