using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerBuff")]
    public class PlayerBuffEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("buff_definition_id")]
        public Guid BuffDefinitionId { get; set; }
        [Column("source_type")]
        public BuffSourceType SourceType { get; set; }
        [Column("source_id")]
        public Guid? SourceId { get; set; }
        [Column("started_at")]
        public DateTime StartedAt { get; set; }
        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }
        [Column("stack_count")]
        public int StackCount { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerBuff_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_PlayerBuff_buff_definition_id </summary>
        public virtual BuffDefinitionEntity BuffDefinition { get; set; }
        #endregion
    }
}