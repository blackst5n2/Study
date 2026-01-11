using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("ShopItemDefinition")]
    public class ShopItemDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("shop_id")]
        public Guid ShopId { get; set; }
        [Column("item_definition_id")]
        public Guid ItemDefinitionId { get; set; }
        [Column("buy_price")]
        public long? BuyPrice { get; set; }
        [Column("sell_price")]
        public long? SellPrice { get; set; }
        [Column("buy_currency_code")]
        public string BuyCurrencyCode { get; set; }
        [Column("sell_currency_code")]
        public string SellCurrencyCode { get; set; }
        [Column("stock")]
        public int? Stock { get; set; }
        [Column("is_limited_time")]
        public bool IsLimitedTime { get; set; }
        [Column("available_start_at")]
        public DateTime? AvailableStartAt { get; set; }
        [Column("available_end_at")]
        public DateTime? AvailableEndAt { get; set; }
        [Column("restock_interval_minutes")]
        public int? RestockIntervalMinutes { get; set; }
        [Column("last_restock_time")]
        public DateTime? LastRestockTime { get; set; }
        [Column("purchase_limit_per_player")]
        public int? PurchaseLimitPerPlayer { get; set; }
        [Column("purchase_limit_interval")]
        public string PurchaseLimitInterval { get; set; }
        [Column("unlock_condition")]
        public string UnlockCondition { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid ShopDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_ShopItemDefinition_shop_id </summary>
        public virtual ShopDefinitionEntity ShopDefinition { get; set; }
        /// <summary> Relation Label: FK_ShopItemDefinition_item_definition_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerShopPurchaseLimit_shop_item_id </summary>
        public virtual ICollection<PlayerShopPurchaseLimitEntity> PlayerShopPurchaseLimits { get; set; } = new HashSet<PlayerShopPurchaseLimitEntity>();
        /// <summary> Relation Label: FK_ShopTransactionLog_shop_item_id </summary>
        public virtual ICollection<ShopTransactionLogEntity> ShopTransactionLogs { get; set; } = new HashSet<ShopTransactionLogEntity>();
        #endregion
    }
}