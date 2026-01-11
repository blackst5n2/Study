namespace Server.Application.DTOs.UserContents;

public class UserPatternBoardDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CustomData { get; set; }
}
