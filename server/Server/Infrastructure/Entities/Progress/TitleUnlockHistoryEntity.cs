using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("TitleUnlockHistory")]
    public class TitleUnlockHistoryEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("title_id")]
        public Guid TitleId { get; set; }
        [Column("event")]
        public TitleUnlockEvent Event { get; set; }
        [Column("occurred_at")]
        public DateTime OccurredAt { get; set; }
        [Column("reason")]
        public string Reason { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_TitleUnlockHistory_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid TitleDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_TitleUnlockHistory_title_id </summary>
        public virtual TitleDefinitionEntity TitleDefinition { get; set; }
        #endregion
    }
}