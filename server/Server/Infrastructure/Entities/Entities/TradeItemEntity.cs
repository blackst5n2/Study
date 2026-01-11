using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("TradeItem")]
    public class TradeItemEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("trade_id")]
        public Guid TradeId { get; set; }
        [Column("offering_player_id")]
        public Guid OfferingPlayerId { get; set; }
        [Column("item_instance_id")]
        public Guid ItemInstanceId { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_TradeItem_trade_id </summary>
        public virtual TradeEntity Trade { get; set; }
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_TradeItem_offering_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_TradeItem_item_instance_id </summary>
        public virtual ItemInstanceEntity ItemInstance { get; set; }
        #endregion
    }
}