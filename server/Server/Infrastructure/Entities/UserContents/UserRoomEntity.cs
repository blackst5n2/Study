using Server.Enums;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.UserContents
{
    [Table("UserRoom")]
    public class UserRoomEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("owner_id")]
        public Guid OwnerId { get; set; }
        [Column("room_name")]
        public string RoomName { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("status")]
        public UserContentStatus Status { get; set; }
        [Column("max_players")]
        public int MaxPlayers { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("map_layout")]
        public string MapLayout { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("last_updated_at")]
        public DateTime LastUpdatedAt { get; set; }
        [Column("like_count")]
        public int LikeCount { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_UserRoom_owner_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_UserRoomParticipant_room_id </summary>
        public virtual ICollection<UserRoomParticipantEntity> UserRoomParticipants { get; set; } = new HashSet<UserRoomParticipantEntity>();
        /// <summary> Relation Label: FK_UserRoomChat_room_id </summary>
        public virtual ICollection<UserRoomChatEntity> UserRoomChats { get; set; } = new HashSet<UserRoomChatEntity>();
        /// <summary> Relation Label: FK_UserRoomFavorite_room_id </summary>
        public virtual ICollection<UserRoomFavoriteEntity> UserRoomFavorites { get; set; } = new HashSet<UserRoomFavoriteEntity>();
        #endregion
    }
}