namespace Server.Application.DTOs.UserContents;

public class UserPatternBoardPostDto
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public Guid PatternId { get; set; }
    public Guid CreatorId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsPinned { get; set; }
    public bool IsDeleted { get; set; }
    public string CustomData { get; set; }
}
