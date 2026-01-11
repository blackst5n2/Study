namespace Server.Application.DTOs.Entities;

public class GuildNoticeDto
{
    public Guid Id { get; set; }
    public Guid GuildId { get; set; }
    public Guid AuthorId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsPinned { get; set; }
    public bool IsActive { get; set; }
    public string CustomData { get; set; }
}
