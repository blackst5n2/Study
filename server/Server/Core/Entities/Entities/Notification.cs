using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public GameNotificationType Type { get; set; }
        public string Payload { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_Notification_player_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}