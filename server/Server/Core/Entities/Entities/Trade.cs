using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Entities
{
    public class Trade
    {
        public Guid Id { get; set; }
        public Guid Player1Id { get; set; }
        public Guid Player2Id { get; set; }
        public bool Player1Accepted { get; set; }
        public bool Player2Accepted { get; set; }
        public string Status { get; set; }
        public DateTime? TradedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CurrencyCode { get; set; }
        public long? Player1CurrencyAmount { get; set; }
        public long? Player2CurrencyAmount { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_Trade_player_1_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_Trade_currency_code </summary>
        public virtual CurrencyDefinition CurrencyDefinition { get; set; }
        /// <summary> Relation Label: FK_TradeItem_trade_id </summary>
        public virtual ICollection<TradeItem> TradeItems { get; set; } = new HashSet<TradeItem>();
        #endregion
    }
}