namespace Server.Application.DTOs.Definitions;

public class JobLevelDefinitionDto
{
    public Guid Id { get; set; }
    public Guid JobId { get; set; }
    public int Level { get; set; }
    public string RequiredExp { get; set; }
    public int RewardSkillPoints { get; set; }
    public Guid? RewardItemId { get; set; }
    public string RewardQuantity { get; set; }
    public string CustomData { get; set; }
}
