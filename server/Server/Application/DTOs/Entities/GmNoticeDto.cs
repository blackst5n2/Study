using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class GmNoticeDto
{
    public Guid Id { get; set; }
    public Guid GmAccountId { get; set; }
    public GmNoticeType NoticeType { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string StartAt { get; set; }
    public string EndAt { get; set; }
    public bool IsActive { get; set; }
    public string TargetAudience { get; set; }
    public string CustomData { get; set; }
}
