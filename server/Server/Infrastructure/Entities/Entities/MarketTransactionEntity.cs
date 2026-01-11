using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("MarketTransaction")]
    public class MarketTransactionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("market_listing_id")]
        public Guid? MarketListingId { get; set; }
        [Column("buyer_id")]
        public Guid BuyerId { get; set; }
        [Column("seller_id")]
        public Guid SellerId { get; set; }
        [Column("item_definition_id")]
        public Guid ItemDefinitionId { get; set; }
        [Column("item_instance_id")]
        public Guid? ItemInstanceId { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("price_per_item")]
        public long PricePerItem { get; set; }
        [Column("total_price")]
        public long TotalPrice { get; set; }
        [Column("currency_code")]
        public string CurrencyCode { get; set; }
        [Column("transacted_at")]
        public DateTime TransactedAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MarketTransaction_market_listing_id </summary>
        public virtual MarketListingEntity MarketListing { get; set; }
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MarketTransaction_buyer_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_MarketTransaction_item_definition_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_MarketTransaction_item_instance_id </summary>
        public virtual ItemInstanceEntity ItemInstance { get; set; }
        public Guid CurrencyDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MarketTransaction_currency_code </summary>
        public virtual CurrencyDefinitionEntity CurrencyDefinition { get; set; }
        #endregion
    }
}