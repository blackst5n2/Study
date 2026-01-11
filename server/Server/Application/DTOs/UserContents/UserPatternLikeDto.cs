namespace Server.Application.DTOs.UserContents;

public class UserPatternLikeDto
{
    public Guid PatternId { get; set; }
    public Guid PlayerId { get; set; }
    public DateTime LikedAt { get; set; }
}
