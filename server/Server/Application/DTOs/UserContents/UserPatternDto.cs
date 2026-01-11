using Server.Enums;
namespace Server.Application.DTOs.UserContents;

public class UserPatternDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public Guid CreatorId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PatternData { get; set; }
    public string PreviewImageUrl { get; set; }
    public string Palette { get; set; }
    public string Tags { get; set; }
    public UserContentStatus Status { get; set; }
    public int LikeCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CustomData { get; set; }
}
