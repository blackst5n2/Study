using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Definitions
{
    public class ShopItemDefinition
    {
        public Guid Id { get; set; }
        public Guid ShopId { get; set; }
        public Guid ItemDefinitionId { get; set; }
        public long? BuyPrice { get; set; }
        public long? SellPrice { get; set; }
        public string BuyCurrencyCode { get; set; }
        public string SellCurrencyCode { get; set; }
        public int? Stock { get; set; }
        public bool IsLimitedTime { get; set; }
        public DateTime? AvailableStartAt { get; set; }
        public DateTime? AvailableEndAt { get; set; }
        public int? RestockIntervalMinutes { get; set; }
        public DateTime? LastRestockTime { get; set; }
        public int? PurchaseLimitPerPlayer { get; set; }
        public string PurchaseLimitInterval { get; set; }
        public string UnlockCondition { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ShopItemDefinition_shop_id </summary>
        public virtual ShopDefinition ShopDefinition { get; set; }
        /// <summary> Relation Label: FK_ShopItemDefinition_item_definition_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerShopPurchaseLimit_shop_item_id </summary>
        public virtual ICollection<PlayerShopPurchaseLimit> PlayerShopPurchaseLimits { get; set; } = new HashSet<PlayerShopPurchaseLimit>();
        /// <summary> Relation Label: FK_ShopTransactionLog_shop_item_id </summary>
        public virtual ICollection<ShopTransactionLog> ShopTransactionLogs { get; set; } = new HashSet<ShopTransactionLog>();
        #endregion
    }
}