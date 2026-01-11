using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class CurrencyDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string Description { get; set; }
        public bool IsPremium { get; set; }
        public string Icon { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MailAttachment_currency_code </summary>
        public virtual ICollection<MailAttachment> MailAttachments { get; set; } = new HashSet<MailAttachment>();
        /// <summary> Relation Label: FK_MarketListing_currency_code </summary>
        public virtual ICollection<MarketListing> MarketListings { get; set; } = new HashSet<MarketListing>();
        /// <summary> Relation Label: FK_MarketTransaction_currency_code </summary>
        public virtual ICollection<MarketTransaction> MarketTransactions { get; set; } = new HashSet<MarketTransaction>();
        /// <summary> Relation Label: FK_Trade_currency_code </summary>
        public virtual ICollection<Trade> Trades { get; set; } = new HashSet<Trade>();
        /// <summary> Relation Label: FK_CurrencyTransactionLog_currency_code </summary>
        public virtual ICollection<CurrencyTransactionLog> CurrencyTransactionLogs { get; set; } = new HashSet<CurrencyTransactionLog>();
        #endregion
    }
}