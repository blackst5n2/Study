using Server.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerInventoryTabLimit")]
    public class PlayerInventoryTabLimitEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("inventory_type")]
        public InventoryType InventoryType { get; set; }
        [Column("max_slots")]
        public int MaxSlots { get; set; }
        [Column("upgraded_at")]
        public DateTime? UpgradedAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerInventoryTabLimit_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}