using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerQuickSlot")]
    public class PlayerQuickSlotEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("preset_id")]
        public Guid PresetId { get; set; }
        [Column("slot_index")]
        public int SlotIndex { get; set; }
        [Column("slot_type")]
        public QuickSlotType SlotType { get; set; }
        [Column("skill_definition_id")]
        public Guid? SkillDefinitionId { get; set; }
        [Column("item_definition_id")]
        public Guid? ItemDefinitionId { get; set; }
        [Column("hotkey")]
        public string Hotkey { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid PlayerQuickSlotPresetId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerQuickSlot_preset_id </summary>
        public virtual PlayerQuickSlotPresetEntity PlayerQuickSlotPreset { get; set; }
        /// <summary> Relation Label: FK_PlayerQuickSlot_skill_definition_id </summary>
        public virtual SkillDefinitionEntity SkillDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerQuickSlot_item_definition_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        #endregion
    }
}