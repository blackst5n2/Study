using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.UserContents
{
    [Table("UserRoomFavorite")]
    public class UserRoomFavoriteEntity
    {
        // 복합키: [player_id, room_id] -> OnModelCreating에서 HasKey 설정 필요
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("room_id")]
        public Guid RoomId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserRoomFavorite_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid UserRoomId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_UserRoomFavorite_room_id </summary>
        public virtual UserRoomEntity UserRoom { get; set; }
        #endregion
    }
}