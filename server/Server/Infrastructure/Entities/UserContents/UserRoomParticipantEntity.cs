using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.UserContents
{
    [Table("UserRoomParticipant")]
    public class UserRoomParticipantEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("room_id")]
        public Guid RoomId { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("role")]
        public string Role { get; set; }
        [Column("joined_at")]
        public DateTime JoinedAt { get; set; }
        [Column("left_at")]
        public DateTime? LeftAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid UserRoomId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_UserRoomParticipant_room_id </summary>
        public virtual UserRoomEntity UserRoom { get; set; }
        /// <summary> Relation Label: FK_UserRoomParticipant_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}