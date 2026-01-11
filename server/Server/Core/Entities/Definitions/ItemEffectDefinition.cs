using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class ItemEffectDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public EffectType EffectType { get; set; }
        public float? ValuePrimary { get; set; }
        public float? ValueSecondary { get; set; }
        public int? DurationSeconds { get; set; }
        public SkillTargetType TargetType { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ItemDefinition_use_effect_id </summary>
        public virtual ICollection<ItemDefinition> ItemDefinitions { get; set; } = new HashSet<ItemDefinition>();
        #endregion
    }
}