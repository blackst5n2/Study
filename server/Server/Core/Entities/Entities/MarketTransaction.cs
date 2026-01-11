using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Entities
{
    public class MarketTransaction
    {
        public Guid Id { get; set; }
        public Guid? MarketListingId { get; set; }
        public Guid BuyerId { get; set; }
        public Guid SellerId { get; set; }
        public Guid ItemDefinitionId { get; set; }
        public Guid? ItemInstanceId { get; set; }
        public int Quantity { get; set; }
        public long PricePerItem { get; set; }
        public long TotalPrice { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime TransactedAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MarketTransaction_market_listing_id </summary>
        public virtual MarketListing MarketListing { get; set; }
        /// <summary> Relation Label: FK_MarketTransaction_buyer_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_MarketTransaction_item_definition_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_MarketTransaction_item_instance_id </summary>
        public virtual ItemInstance ItemInstance { get; set; }
        /// <summary> Relation Label: FK_MarketTransaction_currency_code </summary>
        public virtual CurrencyDefinition CurrencyDefinition { get; set; }
        #endregion
    }
}