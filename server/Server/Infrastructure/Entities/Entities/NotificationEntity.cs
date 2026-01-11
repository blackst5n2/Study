using Server.Enums;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("Notification")]
    public class NotificationEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("type")]
        public GameNotificationType Type { get; set; }
        [Column("payload")]
        public string Payload { get; set; }
        [Column("is_read")]
        public bool IsRead { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_Notification_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}