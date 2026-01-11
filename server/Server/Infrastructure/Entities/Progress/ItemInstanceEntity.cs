using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("ItemInstance")]
    public class ItemInstanceEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("container_id")]
        public Guid ContainerId { get; set; }
        [Column("item_definition_id")]
        public Guid ItemDefinitionId { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("slot_index")]
        public int? SlotIndex { get; set; }
        [Column("durability")]
        public int? Durability { get; set; }
        [Column("max_durability")]
        public int? MaxDurability { get; set; }
        [Column("enhancement_level")]
        public int EnhancementLevel { get; set; }
        [Column("acquired_at")]
        public DateTime AcquiredAt { get; set; }
        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }
        [Column("is_locked")]
        public bool IsLocked { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ItemInstance_item_definition_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_ItemInstance_container_id </summary>
        public virtual ContainerEntity Container { get; set; }
        /// <summary> Relation Label: FK_PlayerInventoryItem_item_instance_id </summary>
        public virtual ICollection<PlayerInventoryItemEntity> PlayerInventoryItems { get; set; } = new HashSet<PlayerInventoryItemEntity>();
        /// <summary> Relation Label: FK_ItemAcquisitionLog_item_instance_id </summary>
        public virtual ICollection<ItemAcquisitionLogEntity> ItemAcquisitionLogs { get; set; } = new HashSet<ItemAcquisitionLogEntity>();
        /// <summary> Relation Label: FK_ItemEnhancementLog_item_instance_id </summary>
        public virtual ICollection<ItemEnhancementLogEntity> ItemEnhancementLogs { get; set; } = new HashSet<ItemEnhancementLogEntity>();
        /// <summary> Relation Label: FK_PlayerCraftingLog_result_item_instance_id </summary>
        public virtual ICollection<PlayerCraftingLogEntity> PlayerCraftingLogs { get; set; } = new HashSet<PlayerCraftingLogEntity>();
        /// <summary> Relation Label: FK_MailAttachment_item_instance_id </summary>
        public virtual ICollection<MailAttachmentEntity> MailAttachments { get; set; } = new HashSet<MailAttachmentEntity>();
        /// <summary> Relation Label: FK_PlayerBuildingSlot_item_instance_id </summary>
        public virtual ICollection<PlayerBuildingSlotEntity> PlayerBuildingSlots { get; set; } = new HashSet<PlayerBuildingSlotEntity>();
        /// <summary> Relation Label: FK_MarketListing_item_instance_id </summary>
        public virtual ICollection<MarketListingEntity> MarketListings { get; set; } = new HashSet<MarketListingEntity>();
        /// <summary> Relation Label: FK_MarketTransaction_item_instance_id </summary>
        public virtual ICollection<MarketTransactionEntity> MarketTransactions { get; set; } = new HashSet<MarketTransactionEntity>();
        /// <summary> Relation Label: FK_TradeItem_item_instance_id </summary>
        public virtual ICollection<TradeItemEntity> TradeItems { get; set; } = new HashSet<TradeItemEntity>();
        #endregion
    }
}