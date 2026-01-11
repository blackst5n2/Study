using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Entities
{
    public class BoardGameParticipant
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid PlayerId { get; set; }
        public DateTime JoinTime { get; set; }
        public DateTime? LeaveTime { get; set; }
        public int? SeatNo { get; set; }
        public bool? IsWinner { get; set; }
        public int? Score { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_BoardGameParticipant_room_id </summary>
        public virtual BoardGameRoom BoardGameRoom { get; set; }
        /// <summary> Relation Label: FK_BoardGameParticipant_player_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}