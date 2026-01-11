using Server.Core.Entities.Definitions;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Entities
{
    public class BoardGameRoom
    {
        public Guid Id { get; set; }
        public Guid BoardGameId { get; set; }
        public string RoomName { get; set; }
        public Guid HostPlayerId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public Guid? WinnerPlayerId { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_BoardGameRoom_board_game_id </summary>
        public virtual BoardGameDefinition BoardGameDefinition { get; set; }
        /// <summary> Relation Label: FK_BoardGameRoom_host_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_BoardGameParticipant_room_id </summary>
        public virtual ICollection<BoardGameParticipant> BoardGameParticipants { get; set; } = new HashSet<BoardGameParticipant>();
        /// <summary> Relation Label: FK_BoardGameTurnLog_room_id </summary>
        public virtual ICollection<BoardGameTurnLog> BoardGameTurnLogs { get; set; } = new HashSet<BoardGameTurnLog>();
        #endregion
    }
}