using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("Trade")]
    public class TradeEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_1_id")]
        public Guid Player1Id { get; set; }
        [Column("player_2_id")]
        public Guid Player2Id { get; set; }
        [Column("player_1_accepted")]
        public bool Player1Accepted { get; set; }
        [Column("player_2_accepted")]
        public bool Player2Accepted { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("traded_at")]
        public DateTime? TradedAt { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("currency_code")]
        public string CurrencyCode { get; set; }
        [Column("player_1_currency_amount")]
        public long? Player1CurrencyAmount { get; set; }
        [Column("player_2_currency_amount")]
        public long? Player2CurrencyAmount { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_Trade_player_1_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid CurrencyDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_Trade_currency_code </summary>
        public virtual CurrencyDefinitionEntity CurrencyDefinition { get; set; }
        /// <summary> Relation Label: FK_TradeItem_trade_id </summary>
        public virtual ICollection<TradeItemEntity> TradeItems { get; set; } = new HashSet<TradeItemEntity>();
        #endregion
    }
}