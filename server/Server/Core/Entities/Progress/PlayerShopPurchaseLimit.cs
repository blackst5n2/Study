using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Progress
{
    public class PlayerShopPurchaseLimit
    {
        public Guid PlayerId { get; set; }
        public Guid ShopItemId { get; set; }
        public string IntervalKey { get; set; }
        public int PurchaseCount { get; set; }
        public DateTime LastPurchaseAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerShopPurchaseLimit_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerShopPurchaseLimit_shop_item_id </summary>
        public virtual ShopItemDefinition ShopItemDefinition { get; set; }
        #endregion
    }
}