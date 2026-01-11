using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class SkillGrantedBuff
    {
        public Guid Id { get; set; }
        public Guid SkillDefinitionId { get; set; }
        public Guid BuffDefinitionId { get; set; }
        public BuffTriggerCondition TriggerCondition { get; set; }
        public float Chance { get; set; }
        public float? DurationOverride { get; set; }
        public string ValueOverride { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_SkillGrantedBuff_skill_definition_id </summary>
        public virtual SkillDefinition SkillDefinition { get; set; }
        /// <summary> Relation Label: FK_SkillGrantedBuff_buff_definition_id </summary>
        public virtual BuffDefinition BuffDefinition { get; set; }
        #endregion
    }
}