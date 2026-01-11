using Server.Enums;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("SkillDefinition")]
    public class SkillDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("skill_type")]
        public string SkillType { get; set; }
        [Column("cooldown_seconds")]
        public float? CooldownSeconds { get; set; }
        [Column("cast_time_seconds")]
        public float? CastTimeSeconds { get; set; }
        [Column("mana_cost")]
        public int? ManaCost { get; set; }
        [Column("target_type")]
        public SkillTargetType TargetType { get; set; }
        [Column("max_level")]
        public int MaxLevel { get; set; }
        [Column("unlock_condition")]
        public string UnlockCondition { get; set; }
        [Column("icon")]
        public string Icon { get; set; }
        [Column("animation_asset")]
        public string AnimationAsset { get; set; }
        [Column("sfx_asset")]
        public string SfxAsset { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_SkillEffectDefinition_skill_definition_id </summary>
        public virtual ICollection<SkillEffectDefinitionEntity> SkillEffectDefinitions { get; set; } = new HashSet<SkillEffectDefinitionEntity>();
        /// <summary> Relation Label: FK_SkillTree_parent_skill_id </summary>
        public virtual ICollection<SkillTreeEntity> SkillTrees { get; set; } = new HashSet<SkillTreeEntity>();
        /// <summary> Relation Label: FK_PlayerSkill_skill_definition_id </summary>
        public virtual ICollection<PlayerSkillEntity> PlayerSkills { get; set; } = new HashSet<PlayerSkillEntity>();
        /// <summary> Relation Label: FK_PlayerSkillLog_skill_definition_id </summary>
        public virtual ICollection<PlayerSkillLogEntity> PlayerSkillLogs { get; set; } = new HashSet<PlayerSkillLogEntity>();
        /// <summary> Relation Label: FK_PlayerQuickSlot_skill_definition_id </summary>
        public virtual ICollection<PlayerQuickSlotEntity> PlayerQuickSlots { get; set; } = new HashSet<PlayerQuickSlotEntity>();
        /// <summary> Relation Label: FK_ClassSkillDefinition_skill_definition_id </summary>
        public virtual ICollection<ClassSkillDefinitionEntity> ClassSkillDefinitions { get; set; } = new HashSet<ClassSkillDefinitionEntity>();
        /// <summary> Relation Label: FK_ClassSkillTree_parent_skill_id </summary>
        public virtual ICollection<ClassSkillTreeEntity> ClassSkillTrees { get; set; } = new HashSet<ClassSkillTreeEntity>();
        /// <summary> Relation Label: FK_PlayerSkillChangeLog_skill_definition_id </summary>
        public virtual ICollection<PlayerSkillChangeLogEntity> PlayerSkillChangeLogs { get; set; } = new HashSet<PlayerSkillChangeLogEntity>();
        /// <summary> Relation Label: FK_SkillGrantedBuff_skill_definition_id </summary>
        public virtual ICollection<SkillGrantedBuffEntity> SkillGrantedBuffs { get; set; } = new HashSet<SkillGrantedBuffEntity>();
        /// <summary> Relation Label: FK_PetSkill_skill_definition_id </summary>
        public virtual ICollection<PetSkillEntity> PetSkills { get; set; } = new HashSet<PetSkillEntity>();
        #endregion
    }
}