using Server.Core.Entities.Definitions;
using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;

namespace Server.Core.Entities.Progress
{
    public class ItemInstance
    {
        public Guid Id { get; set; }
        public Guid ContainerId { get; set; }
        public Guid ItemDefinitionId { get; set; }
        public int Quantity { get; set; }
        public int? SlotIndex { get; set; }
        public int? Durability { get; set; }
        public int? MaxDurability { get; set; }
        public int EnhancementLevel { get; set; }
        public DateTime AcquiredAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsLocked { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ItemInstance_item_definition_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_ItemInstance_container_id </summary>
        public virtual Container Container { get; set; }
        /// <summary> Relation Label: FK_PlayerInventoryItem_item_instance_id </summary>
        public virtual ICollection<PlayerInventoryItem> PlayerInventoryItems { get; set; } = new HashSet<PlayerInventoryItem>();
        /// <summary> Relation Label: FK_ItemAcquisitionLog_item_instance_id </summary>
        public virtual ICollection<ItemAcquisitionLog> ItemAcquisitionLogs { get; set; } = new HashSet<ItemAcquisitionLog>();
        /// <summary> Relation Label: FK_ItemEnhancementLog_item_instance_id </summary>
        public virtual ICollection<ItemEnhancementLog> ItemEnhancementLogs { get; set; } = new HashSet<ItemEnhancementLog>();
        /// <summary> Relation Label: FK_PlayerCraftingLog_result_item_instance_id </summary>
        public virtual ICollection<PlayerCraftingLog> PlayerCraftingLogs { get; set; } = new HashSet<PlayerCraftingLog>();
        /// <summary> Relation Label: FK_MailAttachment_item_instance_id </summary>
        public virtual ICollection<MailAttachment> MailAttachments { get; set; } = new HashSet<MailAttachment>();
        /// <summary> Relation Label: FK_PlayerBuildingSlot_item_instance_id </summary>
        public virtual ICollection<PlayerBuildingSlot> PlayerBuildingSlots { get; set; } = new HashSet<PlayerBuildingSlot>();
        /// <summary> Relation Label: FK_MarketListing_item_instance_id </summary>
        public virtual ICollection<MarketListing> MarketListings { get; set; } = new HashSet<MarketListing>();
        /// <summary> Relation Label: FK_MarketTransaction_item_instance_id </summary>
        public virtual ICollection<MarketTransaction> MarketTransactions { get; set; } = new HashSet<MarketTransaction>();
        /// <summary> Relation Label: FK_TradeItem_item_instance_id </summary>
        public virtual ICollection<TradeItem> TradeItems { get; set; } = new HashSet<TradeItem>();
        #endregion
    }
}