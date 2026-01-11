using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerShopPurchaseLimit")]
    public class PlayerShopPurchaseLimitEntity
    {
        // 복합키: [player_id, shop_item_id, interval_key] -> OnModelCreating에서 HasKey 설정 필요
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("shop_item_id")]
        public Guid ShopItemId { get; set; }
        [Column("interval_key")]
        public string IntervalKey { get; set; }
        [Column("purchase_count")]
        public int PurchaseCount { get; set; }
        [Column("last_purchase_at")]
        public DateTime LastPurchaseAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerShopPurchaseLimit_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid ShopItemDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerShopPurchaseLimit_shop_item_id </summary>
        public virtual ShopItemDefinitionEntity ShopItemDefinition { get; set; }
        #endregion
    }
}