using Server.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("ItemEffectDefinition")]
    public class ItemEffectDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("effect_type")]
        public EffectType EffectType { get; set; }
        [Column("value_primary")]
        public float? ValuePrimary { get; set; }
        [Column("value_secondary")]
        public float? ValueSecondary { get; set; }
        [Column("duration_seconds")]
        public int? DurationSeconds { get; set; }
        [Column("target_type")]
        public SkillTargetType TargetType { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ItemDefinition_use_effect_id </summary>
        public virtual ICollection<ItemDefinitionEntity> ItemDefinitions { get; set; } = new HashSet<ItemDefinitionEntity>();
        #endregion
    }
}