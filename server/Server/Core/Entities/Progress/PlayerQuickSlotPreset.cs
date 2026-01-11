
namespace Server.Core.Entities.Progress
{
    public class PlayerQuickSlotPreset
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
        public int PresetIndex { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerQuickSlotPreset_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerQuickSlot_preset_id </summary>
        public virtual ICollection<PlayerQuickSlot> PlayerQuickSlots { get; set; } = new HashSet<PlayerQuickSlot>();
        #endregion
    }
}