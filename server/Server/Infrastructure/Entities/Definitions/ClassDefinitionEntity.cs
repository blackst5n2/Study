using Server.Enums;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("ClassDefinition")]
    public class ClassDefinitionEntity
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
        [Column("parent_class_id")]
        public Guid? ParentClassId { get; set; }
        [Column("unlock_condition")]
        public string UnlockCondition { get; set; }
        [Column("is_base")]
        public bool IsBase { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("icon")]
        public string Icon { get; set; }
        [Column("role")]
        public ClassRole Role { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid ParentId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_ClassDefinition_parent_class_id </summary>
        public virtual ClassDefinitionEntity Parent { get; set; }
        /// <summary> Relation Label: FK_ClassDefinition_parent_class_id </summary>
        public virtual ICollection<ClassDefinitionEntity> Children { get; set; } = new HashSet<ClassDefinitionEntity>();
        /// <summary> Relation Label: FK_ClassTree_parent_class_id </summary>
        public virtual ICollection<ClassTreeEntity> ClassTrees { get; set; } = new HashSet<ClassTreeEntity>();
        /// <summary> Relation Label: FK_PlayerClass_class_id </summary>
        public virtual ICollection<PlayerClassEntity> PlayerClasses { get; set; } = new HashSet<PlayerClassEntity>();
        /// <summary> Relation Label: FK_ClassEquipmentRestriction_class_id </summary>
        public virtual ICollection<ClassEquipmentRestrictionEntity> ClassEquipmentRestrictions { get; set; } = new HashSet<ClassEquipmentRestrictionEntity>();
        /// <summary> Relation Label: FK_ClassTraitDefinition_class_id </summary>
        public virtual ICollection<ClassTraitDefinitionEntity> ClassTraitDefinitions { get; set; } = new HashSet<ClassTraitDefinitionEntity>();
        /// <summary> Relation Label: FK_ClassSkillDefinition_class_id </summary>
        public virtual ICollection<ClassSkillDefinitionEntity> ClassSkillDefinitions { get; set; } = new HashSet<ClassSkillDefinitionEntity>();
        /// <summary> Relation Label: FK_ClassSkillTree_class_id </summary>
        public virtual ICollection<ClassSkillTreeEntity> ClassSkillTrees { get; set; } = new HashSet<ClassSkillTreeEntity>();
        /// <summary> Relation Label: FK_PlayerClassChangeLog_old_class_id </summary>
        public virtual ICollection<PlayerClassChangeLogEntity> PlayerClassChangeLogs { get; set; } = new HashSet<PlayerClassChangeLogEntity>();
        /// <summary> Relation Label: N:M via QuestClassMapping </summary>
        public virtual ICollection<QuestDefinitionEntity> QuestDefinitions { get; set; } = new HashSet<QuestDefinitionEntity>();
        /// <summary> Relation Label: N:M via AchievementClassMapping </summary>
        public virtual ICollection<AchievementDefinitionEntity> AchievementDefinitions { get; set; } = new HashSet<AchievementDefinitionEntity>();
        #endregion
    }
}