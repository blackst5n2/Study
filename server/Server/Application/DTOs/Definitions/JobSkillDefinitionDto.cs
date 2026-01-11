namespace Server.Application.DTOs.Definitions;

public class JobSkillDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public Guid JobId { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string UnlockCondition { get; set; }
    public string Icon { get; set; }
    public int MaxLevel { get; set; }
    public string CustomData { get; set; }
}
