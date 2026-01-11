using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class NotificationSchedule
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationScheduleType Type { get; set; }
        public NotificationTargetType TargetType { get; set; }
        public Guid? TargetId { get; set; }
        public string TargetSegment { get; set; }
        public DateTime ScheduledAt { get; set; }
        public DateTime? SentAt { get; set; }
        public NotificationStatus Status { get; set; }
        public int RetryCount { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_NotificationLog_notification_schedule_id </summary>
        public virtual ICollection<NotificationLog> NotificationLogs { get; set; } = new HashSet<NotificationLog>();
        #endregion
    }
}