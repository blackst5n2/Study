using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class NotificationScheduleDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public NotificationScheduleType Type { get; set; }
    public NotificationTargetType TargetType { get; set; }
    public Guid? TargetId { get; set; }
    public string TargetSegment { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string SentAt { get; set; }
    public NotificationStatus Status { get; set; }
    public int RetryCount { get; set; }
    public string CustomData { get; set; }
}
