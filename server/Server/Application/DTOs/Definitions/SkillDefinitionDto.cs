using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class SkillDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string SkillType { get; set; }
    public string CooldownSeconds { get; set; }
    public string CastTimeSeconds { get; set; }
    public string ManaCost { get; set; }
    public SkillTargetType TargetType { get; set; }
    public int MaxLevel { get; set; }
    public string UnlockCondition { get; set; }
    public string Icon { get; set; }
    public string AnimationAsset { get; set; }
    public string SfxAsset { get; set; }
    public string CustomData { get; set; }
}
