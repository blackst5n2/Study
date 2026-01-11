using Server.Core.Entities.Progress;

namespace Server.Core.Entities.UserContents
{
    public class UserRoomChat
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid PlayerId { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserRoomChat_room_id </summary>
        public virtual UserRoom UserRoom { get; set; }
        /// <summary> Relation Label: FK_UserRoomChat_player_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}