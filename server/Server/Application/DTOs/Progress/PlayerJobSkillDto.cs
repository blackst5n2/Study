namespace Server.Application.DTOs.Progress;

public class PlayerJobSkillDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid SkillId { get; set; }
    public int Level { get; set; }
    public DateTime AcquiredAt { get; set; }
    public string CustomData { get; set; }
}
