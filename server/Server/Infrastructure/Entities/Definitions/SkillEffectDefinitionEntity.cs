using Server.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("SkillEffectDefinition")]
    public class SkillEffectDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("skill_definition_id")]
        public Guid SkillDefinitionId { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("effect_type")]
        public SkillEffectType EffectType { get; set; }
        [Column("value_formula")]
        public string ValueFormula { get; set; }
        [Column("duration_seconds")]
        public float? DurationSeconds { get; set; }
        [Column("target_type")]
        public SkillTargetType TargetType { get; set; }
        [Column("condition")]
        public string Condition { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_SkillEffectDefinition_skill_definition_id </summary>
        public virtual SkillDefinitionEntity SkillDefinition { get; set; }
        #endregion
    }
}