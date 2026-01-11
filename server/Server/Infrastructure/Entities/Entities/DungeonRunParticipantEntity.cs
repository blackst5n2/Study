using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("DungeonRunParticipant")]
    public class DungeonRunParticipantEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("dungeon_run_id")]
        public Guid DungeonRunId { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("joined_at")]
        public DateTime JoinedAt { get; set; }
        [Column("left_at")]
        public DateTime? LeftAt { get; set; }
        [Column("is_cleared")]
        public bool IsCleared { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_DungeonRunParticipant_dungeon_run_id </summary>
        public virtual DungeonRunEntity DungeonRun { get; set; }
        /// <summary> Relation Label: FK_DungeonRunParticipant_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}