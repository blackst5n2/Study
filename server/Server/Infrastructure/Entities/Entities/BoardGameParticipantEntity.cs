using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("BoardGameParticipant")]
    public class BoardGameParticipantEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("room_id")]
        public Guid RoomId { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("join_time")]
        public DateTime JoinTime { get; set; }
        [Column("leave_time")]
        public DateTime? LeaveTime { get; set; }
        [Column("seat_no")]
        public int? SeatNo { get; set; }
        [Column("is_winner")]
        public bool? IsWinner { get; set; }
        [Column("score")]
        public int? Score { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid BoardGameRoomId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_BoardGameParticipant_room_id </summary>
        public virtual BoardGameRoomEntity BoardGameRoom { get; set; }
        /// <summary> Relation Label: FK_BoardGameParticipant_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}