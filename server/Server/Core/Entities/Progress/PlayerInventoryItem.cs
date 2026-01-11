using Server.Enums;

namespace Server.Core.Entities.Progress
{
    public class PlayerInventoryItem
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid ItemInstanceId { get; set; }
        public InventoryType InventoryType { get; set; }
        public int SlotIndex { get; set; }
        public InventoryItemStatus Status { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerInventoryItem_item_instance_id </summary>
        public virtual ItemInstance ItemInstance { get; set; }
        /// <summary> Relation Label: FK_PlayerInventoryItem_player_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}