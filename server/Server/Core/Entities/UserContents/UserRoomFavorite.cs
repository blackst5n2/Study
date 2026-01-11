using Server.Core.Entities.Progress;

namespace Server.Core.Entities.UserContents
{
    public class UserRoomFavorite
    {
        public Guid PlayerId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime CreatedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserRoomFavorite_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_UserRoomFavorite_room_id </summary>
        public virtual UserRoom UserRoom { get; set; }
        #endregion
    }
}