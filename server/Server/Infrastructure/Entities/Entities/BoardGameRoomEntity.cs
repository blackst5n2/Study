using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("BoardGameRoom")]
    public class BoardGameRoomEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("board_game_id")]
        public Guid BoardGameId { get; set; }
        [Column("room_name")]
        public string RoomName { get; set; }
        [Column("host_player_id")]
        public Guid HostPlayerId { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("started_at")]
        public DateTime? StartedAt { get; set; }
        [Column("ended_at")]
        public DateTime? EndedAt { get; set; }
        [Column("winner_player_id")]
        public Guid? WinnerPlayerId { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid BoardGameDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_BoardGameRoom_board_game_id </summary>
        public virtual BoardGameDefinitionEntity BoardGameDefinition { get; set; }
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_BoardGameRoom_host_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_BoardGameParticipant_room_id </summary>
        public virtual ICollection<BoardGameParticipantEntity> BoardGameParticipants { get; set; } = new HashSet<BoardGameParticipantEntity>();
        /// <summary> Relation Label: FK_BoardGameTurnLog_room_id </summary>
        public virtual ICollection<BoardGameTurnLogEntity> BoardGameTurnLogs { get; set; } = new HashSet<BoardGameTurnLogEntity>();
        #endregion
    }
}