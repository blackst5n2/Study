using Server.Enums;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("CurrencyDefinition")]
    public class CurrencyDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("currency_type")]
        public CurrencyType CurrencyType { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("is_premium")]
        public bool IsPremium { get; set; }
        [Column("icon")]
        public string Icon { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MailAttachment_currency_code </summary>
        public virtual ICollection<MailAttachmentEntity> MailAttachments { get; set; } = new HashSet<MailAttachmentEntity>();
        /// <summary> Relation Label: FK_MarketListing_currency_code </summary>
        public virtual ICollection<MarketListingEntity> MarketListings { get; set; } = new HashSet<MarketListingEntity>();
        /// <summary> Relation Label: FK_MarketTransaction_currency_code </summary>
        public virtual ICollection<MarketTransactionEntity> MarketTransactions { get; set; } = new HashSet<MarketTransactionEntity>();
        /// <summary> Relation Label: FK_Trade_currency_code </summary>
        public virtual ICollection<TradeEntity> Trades { get; set; } = new HashSet<TradeEntity>();
        /// <summary> Relation Label: FK_CurrencyTransactionLog_currency_code </summary>
        public virtual ICollection<CurrencyTransactionLogEntity> CurrencyTransactionLogs { get; set; } = new HashSet<CurrencyTransactionLogEntity>();
        #endregion
    }
}