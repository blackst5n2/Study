using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class MarketListing
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public Guid ItemDefinitionId { get; set; }
        public Guid? ItemInstanceId { get; set; }
        public int Quantity { get; set; }
        public long PricePerItem { get; set; }
        public string CurrencyCode { get; set; }
        public MarketListingStatus Status { get; set; }
        public DateTime ListedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime? SoldAt { get; set; }
        public Guid? BuyerId { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MarketListing_seller_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_MarketListing_item_definition_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_MarketListing_item_instance_id </summary>
        public virtual ItemInstance ItemInstance { get; set; }
        /// <summary> Relation Label: FK_MarketListing_currency_code </summary>
        public virtual CurrencyDefinition CurrencyDefinition { get; set; }
        /// <summary> Relation Label: FK_MarketTransaction_market_listing_id </summary>
        public virtual ICollection<MarketTransaction> MarketTransactions { get; set; } = new HashSet<MarketTransaction>();
        #endregion
    }
}