using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.UserContents
{
    [Table("UserRoomChat")]
    public class UserRoomChatEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("room_id")]
        public Guid RoomId { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("sent_at")]
        public DateTime SentAt { get; set; }

        #region Navigation Properties
        public Guid UserRoomId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_UserRoomChat_room_id </summary>
        public virtual UserRoomEntity UserRoom { get; set; }
        /// <summary> Relation Label: FK_UserRoomChat_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}