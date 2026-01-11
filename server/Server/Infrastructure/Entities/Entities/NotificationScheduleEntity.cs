using Server.Enums;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("NotificationSchedule")]
    public class NotificationScheduleEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("type")]
        public NotificationScheduleType Type { get; set; }
        [Column("target_type")]
        public NotificationTargetType TargetType { get; set; }
        [Column("target_id")]
        public Guid? TargetId { get; set; }
        [Column("target_segment")]
        public string TargetSegment { get; set; }
        [Column("scheduled_at")]
        public DateTime ScheduledAt { get; set; }
        [Column("sent_at")]
        public DateTime? SentAt { get; set; }
        [Column("status")]
        public NotificationStatus Status { get; set; }
        [Column("retry_count")]
        public int RetryCount { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_NotificationLog_notification_schedule_id </summary>
        public virtual ICollection<NotificationLogEntity> NotificationLogs { get; set; } = new HashSet<NotificationLogEntity>();
        #endregion
    }
}