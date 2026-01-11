using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class SkillEffectDefinitionDto
{
    public Guid Id { get; set; }
    public Guid SkillDefinitionId { get; set; }
    public int Level { get; set; }
    public SkillEffectType EffectType { get; set; }
    public string ValueFormula { get; set; }
    public string DurationSeconds { get; set; }
    public SkillTargetType TargetType { get; set; }
    public string Condition { get; set; }
    public string CustomData { get; set; }
}
