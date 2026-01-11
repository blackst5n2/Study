namespace Server.Application.DTOs.Progress;

public class PlayerJobSkillPointDto
{
    public Guid PlayerId { get; set; }
    public Guid JobId { get; set; }
    public int TotalPoints { get; set; }
    public int UsedPoints { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string CustomData { get; set; }
}
