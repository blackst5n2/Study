namespace Server.Application.DTOs.UserContents;

public class UserPatternFavoriteDto
{
    public Guid PlayerId { get; set; }
    public Guid PatternId { get; set; }
    public DateTime CreatedAt { get; set; }
}
