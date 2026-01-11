using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Progress
{
    public class PlayerQuickSlot
    {
        public Guid Id { get; set; }
        public Guid PresetId { get; set; }
        public int SlotIndex { get; set; }
        public QuickSlotType SlotType { get; set; }
        public Guid? SkillDefinitionId { get; set; }
        public Guid? ItemDefinitionId { get; set; }
        public string Hotkey { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerQuickSlot_preset_id </summary>
        public virtual PlayerQuickSlotPreset PlayerQuickSlotPreset { get; set; }
        /// <summary> Relation Label: FK_PlayerQuickSlot_skill_definition_id </summary>
        public virtual SkillDefinition SkillDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerQuickSlot_item_definition_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        #endregion
    }
}