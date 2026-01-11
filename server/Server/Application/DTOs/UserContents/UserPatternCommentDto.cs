namespace Server.Application.DTOs.UserContents;

public class UserPatternCommentDto
{
    public Guid Id { get; set; }
    public Guid PatternId { get; set; }
    public Guid PlayerId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
