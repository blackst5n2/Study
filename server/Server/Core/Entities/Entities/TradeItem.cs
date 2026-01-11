using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Entities
{
    public class TradeItem
    {
        public Guid Id { get; set; }
        public Guid TradeId { get; set; }
        public Guid OfferingPlayerId { get; set; }
        public Guid ItemInstanceId { get; set; }
        public int Quantity { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_TradeItem_trade_id </summary>
        public virtual Trade Trade { get; set; }
        /// <summary> Relation Label: FK_TradeItem_offering_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_TradeItem_item_instance_id </summary>
        public virtual ItemInstance ItemInstance { get; set; }
        #endregion
    }
}