using Server.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerInventoryItem")]
    public class PlayerInventoryItemEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("item_instance_id")]
        public Guid ItemInstanceId { get; set; }
        [Column("inventory_type")]
        public InventoryType InventoryType { get; set; }
        [Column("slot_index")]
        public int SlotIndex { get; set; }
        [Column("status")]
        public InventoryItemStatus Status { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerInventoryItem_item_instance_id </summary>
        public virtual ItemInstanceEntity ItemInstance { get; set; }
        /// <summary> Relation Label: FK_PlayerInventoryItem_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}