using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("SkillGrantedBuff")]
    public class SkillGrantedBuffEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("skill_definition_id")]
        public Guid SkillDefinitionId { get; set; }
        [Column("buff_definition_id")]
        public Guid BuffDefinitionId { get; set; }
        [Column("trigger_condition")]
        public BuffTriggerCondition TriggerCondition { get; set; }
        [Column("chance")]
        public float Chance { get; set; }
        [Column("duration_override")]
        public float? DurationOverride { get; set; }
        [Column("value_override")]
        public string ValueOverride { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_SkillGrantedBuff_skill_definition_id </summary>
        public virtual SkillDefinitionEntity SkillDefinition { get; set; }
        /// <summary> Relation Label: FK_SkillGrantedBuff_buff_definition_id </summary>
        public virtual BuffDefinitionEntity BuffDefinition { get; set; }
        #endregion
    }
}