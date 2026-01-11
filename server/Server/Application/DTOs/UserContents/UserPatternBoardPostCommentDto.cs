namespace Server.Application.DTOs.UserContents;

public class UserPatternBoardPostCommentDto
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid CommenterId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
