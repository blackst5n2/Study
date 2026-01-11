using Server.Core.Entities.Progress;

namespace Server.Core.Entities.UserContents
{
    public class UserRoomParticipant
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid PlayerId { get; set; }
        public string Role { get; set; }
        public DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserRoomParticipant_room_id </summary>
        public virtual UserRoom UserRoom { get; set; }
        /// <summary> Relation Label: FK_UserRoomParticipant_player_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}