using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class ItemEffectDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public EffectType EffectType { get; set; }
    public string ValuePrimary { get; set; }
    public string ValueSecondary { get; set; }
    public string DurationSeconds { get; set; }
    public SkillTargetType TargetType { get; set; }
    public string CustomData { get; set; }
}
