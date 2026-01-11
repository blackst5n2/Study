using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerTitle")]
    public class PlayerTitleEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("title_id")]
        public Guid TitleId { get; set; }
        [Column("acquired_at")]
        public DateTime AcquiredAt { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerTitle_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid TitleDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerTitle_title_id </summary>
        public virtual TitleDefinitionEntity TitleDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerTitleSlot_player_title_id </summary>
        public virtual ICollection<PlayerTitleSlotEntity> PlayerTitleSlots { get; set; } = new HashSet<PlayerTitleSlotEntity>();
        #endregion
    }
}