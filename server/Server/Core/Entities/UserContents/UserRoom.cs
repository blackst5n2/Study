using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.UserContents
{
    public class UserRoom
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string RoomName { get; set; }
        public string Description { get; set; }
        public UserContentStatus Status { get; set; }
        public int MaxPlayers { get; set; }
        public string Password { get; set; }
        public string MapLayout { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public int LikeCount { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserRoom_owner_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_UserRoomParticipant_room_id </summary>
        public virtual ICollection<UserRoomParticipant> UserRoomParticipants { get; set; } = new HashSet<UserRoomParticipant>();
        /// <summary> Relation Label: FK_UserRoomChat_room_id </summary>
        public virtual ICollection<UserRoomChat> UserRoomChats { get; set; } = new HashSet<UserRoomChat>();
        /// <summary> Relation Label: FK_UserRoomFavorite_room_id </summary>
        public virtual ICollection<UserRoomFavorite> UserRoomFavorites { get; set; } = new HashSet<UserRoomFavorite>();
        #endregion
    }
}