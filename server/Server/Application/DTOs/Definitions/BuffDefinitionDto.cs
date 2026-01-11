using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class BuffDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public BuffCategory Category { get; set; }
    public BuffType EffectType { get; set; }
    public string ValueFormula { get; set; }
    public string DurationSeconds { get; set; }
    public int MaxStack { get; set; }
    public bool IsDispellable { get; set; }
    public string Icon { get; set; }
    public string ParticleEffect { get; set; }
    public string CustomData { get; set; }
}
