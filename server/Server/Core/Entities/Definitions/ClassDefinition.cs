using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class ClassDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentClassId { get; set; }
        public string UnlockCondition { get; set; }
        public bool IsBase { get; set; }
        public bool IsActive { get; set; }
        public string Icon { get; set; }
        public ClassRole Role { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ClassDefinition_parent_class_id </summary>
        public virtual ClassDefinition Parent { get; set; }
        /// <summary> Relation Label: FK_ClassDefinition_parent_class_id </summary>
        public virtual ICollection<ClassDefinition> Children { get; set; } = new HashSet<ClassDefinition>();
        /// <summary> Relation Label: FK_ClassTree_parent_class_id </summary>
        public virtual ICollection<ClassTree> ClassTrees { get; set; } = new HashSet<ClassTree>();
        /// <summary> Relation Label: FK_PlayerClass_class_id </summary>
        public virtual ICollection<PlayerClass> PlayerClasses { get; set; } = new HashSet<PlayerClass>();
        /// <summary> Relation Label: FK_ClassEquipmentRestriction_class_id </summary>
        public virtual ICollection<ClassEquipmentRestriction> ClassEquipmentRestrictions { get; set; } = new HashSet<ClassEquipmentRestriction>();
        /// <summary> Relation Label: FK_ClassTraitDefinition_class_id </summary>
        public virtual ICollection<ClassTraitDefinition> ClassTraitDefinitions { get; set; } = new HashSet<ClassTraitDefinition>();
        /// <summary> Relation Label: FK_ClassSkillDefinition_class_id </summary>
        public virtual ICollection<ClassSkillDefinition> ClassSkillDefinitions { get; set; } = new HashSet<ClassSkillDefinition>();
        /// <summary> Relation Label: FK_ClassSkillTree_class_id </summary>
        public virtual ICollection<ClassSkillTree> ClassSkillTrees { get; set; } = new HashSet<ClassSkillTree>();
        /// <summary> Relation Label: FK_PlayerClassChangeLog_old_class_id </summary>
        public virtual ICollection<PlayerClassChangeLog> PlayerClassChangeLogs { get; set; } = new HashSet<PlayerClassChangeLog>();
        /// <summary> Relation Label: N:M via QuestClassMapping </summary>
        public virtual ICollection<QuestDefinition> QuestDefinitions { get; set; } = new HashSet<QuestDefinition>();
        /// <summary> Relation Label: N:M via AchievementClassMapping </summary>
        public virtual ICollection<AchievementDefinition> AchievementDefinitions { get; set; } = new HashSet<AchievementDefinition>();
        #endregion
    }
}