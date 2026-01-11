using Server.Enums;

namespace Server.Core.Entities.Progress
{
    public class PlayerInventoryTabLimit
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public InventoryType InventoryType { get; set; }
        public int MaxSlots { get; set; }
        public DateTime? UpgradedAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerInventoryTabLimit_player_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}