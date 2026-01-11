using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class SkillDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SkillType { get; set; }
        public float? CooldownSeconds { get; set; }
        public float? CastTimeSeconds { get; set; }
        public int? ManaCost { get; set; }
        public SkillTargetType TargetType { get; set; }
        public int MaxLevel { get; set; }
        public string UnlockCondition { get; set; }
        public string Icon { get; set; }
        public string AnimationAsset { get; set; }
        public string SfxAsset { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_SkillEffectDefinition_skill_definition_id </summary>
        public virtual ICollection<SkillEffectDefinition> SkillEffectDefinitions { get; set; } = new HashSet<SkillEffectDefinition>();
        /// <summary> Relation Label: FK_SkillTree_parent_skill_id </summary>
        public virtual ICollection<SkillTree> SkillTrees { get; set; } = new HashSet<SkillTree>();
        /// <summary> Relation Label: FK_PlayerSkill_skill_definition_id </summary>
        public virtual ICollection<PlayerSkill> PlayerSkills { get; set; } = new HashSet<PlayerSkill>();
        /// <summary> Relation Label: FK_PlayerSkillLog_skill_definition_id </summary>
        public virtual ICollection<PlayerSkillLog> PlayerSkillLogs { get; set; } = new HashSet<PlayerSkillLog>();
        /// <summary> Relation Label: FK_PlayerQuickSlot_skill_definition_id </summary>
        public virtual ICollection<PlayerQuickSlot> PlayerQuickSlots { get; set; } = new HashSet<PlayerQuickSlot>();
        /// <summary> Relation Label: FK_ClassSkillDefinition_skill_definition_id </summary>
        public virtual ICollection<ClassSkillDefinition> ClassSkillDefinitions { get; set; } = new HashSet<ClassSkillDefinition>();
        /// <summary> Relation Label: FK_ClassSkillTree_parent_skill_id </summary>
        public virtual ICollection<ClassSkillTree> ClassSkillTrees { get; set; } = new HashSet<ClassSkillTree>();
        /// <summary> Relation Label: FK_PlayerSkillChangeLog_skill_definition_id </summary>
        public virtual ICollection<PlayerSkillChangeLog> PlayerSkillChangeLogs { get; set; } = new HashSet<PlayerSkillChangeLog>();
        /// <summary> Relation Label: FK_SkillGrantedBuff_skill_definition_id </summary>
        public virtual ICollection<SkillGrantedBuff> SkillGrantedBuffs { get; set; } = new HashSet<SkillGrantedBuff>();
        /// <summary> Relation Label: FK_PetSkill_skill_definition_id </summary>
        public virtual ICollection<PetSkill> PetSkills { get; set; } = new HashSet<PetSkill>();
        #endregion
    }
}