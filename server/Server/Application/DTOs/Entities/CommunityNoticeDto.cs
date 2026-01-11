using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class CommunityNoticeDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public NoticeType Type { get; set; }
    public Guid? AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string StartAt { get; set; }
    public string EndAt { get; set; }
    public bool IsActive { get; set; }
    public string TargetChannel { get; set; }
    public string CustomData { get; set; }
}
