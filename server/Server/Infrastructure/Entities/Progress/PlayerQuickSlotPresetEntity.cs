using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerQuickSlotPreset")]
    public class PlayerQuickSlotPresetEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("preset_index")]
        public int PresetIndex { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerQuickSlotPreset_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_PlayerQuickSlot_preset_id </summary>
        public virtual ICollection<PlayerQuickSlotEntity> PlayerQuickSlots { get; set; } = new HashSet<PlayerQuickSlotEntity>();
        #endregion
    }
}