namespace Server.Application.DTOs.Progress;

public class PlayerSkillDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid SkillDefinitionId { get; set; }
    public int Level { get; set; }
    public bool IsLearned { get; set; }
    public string AcquiredAt { get; set; }
    public string LastUsedAt { get; set; }
    public string CustomData { get; set; }
}
