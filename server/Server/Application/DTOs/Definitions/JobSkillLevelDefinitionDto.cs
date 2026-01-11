namespace Server.Application.DTOs.Definitions;

public class JobSkillLevelDefinitionDto
{
    public Guid Id { get; set; }
    public Guid SkillId { get; set; }
    public int Level { get; set; }
    public string RequiredJobLevel { get; set; }
    public int RequiredSkillPoints { get; set; }
    public string EffectDescription { get; set; }
    public string EffectData { get; set; }
    public Guid? RewardItemId { get; set; }
    public string RewardQuantity { get; set; }
    public string CustomData { get; set; }
}
