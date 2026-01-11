using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerEventProgress")]
    public class PlayerEventProgressEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("event_id")]
        public Guid EventId { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }

        #region Navigation Properties
        public Guid EventDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerEventProgress_event_id </summary>
        public virtual EventDefinitionEntity EventDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerEventProgress_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}