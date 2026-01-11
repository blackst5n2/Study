using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerTitleSlot")]
    public class PlayerTitleSlotEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("slot_index")]
        public int SlotIndex { get; set; }
        [Column("player_title_id")]
        public Guid? PlayerTitleId { get; set; }
        [Column("equipped_at")]
        public DateTime? EquippedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerTitleSlot_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_PlayerTitleSlot_player_title_id </summary>
        public virtual PlayerTitleEntity PlayerTitle { get; set; }
        #endregion
    }
}