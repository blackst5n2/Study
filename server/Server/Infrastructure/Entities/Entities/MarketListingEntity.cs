using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("MarketListing")]
    public class MarketListingEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
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
        [Column("currency_code")]
        public string CurrencyCode { get; set; }
        [Column("status")]
        public MarketListingStatus Status { get; set; }
        [Column("listed_at")]
        public DateTime ListedAt { get; set; }
        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }
        [Column("sold_at")]
        public DateTime? SoldAt { get; set; }
        [Column("buyer_id")]
        public Guid? BuyerId { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MarketListing_seller_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_MarketListing_item_definition_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_MarketListing_item_instance_id </summary>
        public virtual ItemInstanceEntity ItemInstance { get; set; }
        public Guid CurrencyDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MarketListing_currency_code </summary>
        public virtual CurrencyDefinitionEntity CurrencyDefinition { get; set; }
        /// <summary> Relation Label: FK_MarketTransaction_market_listing_id </summary>
        public virtual ICollection<MarketTransactionEntity> MarketTransactions { get; set; } = new HashSet<MarketTransactionEntity>();
        #endregion
    }
}