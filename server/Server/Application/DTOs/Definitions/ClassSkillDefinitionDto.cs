namespace Server.Application.DTOs.Definitions;

public class ClassSkillDefinitionDto
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public Guid SkillDefinitionId { get; set; }
    public string UnlockLevel { get; set; }
    public string UnlockCondition { get; set; }
    public bool IsActive { get; set; }
    public string CustomData { get; set; }
}
