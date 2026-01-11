using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class SkillGrantedBuffDto
{
    public Guid Id { get; set; }
    public Guid SkillDefinitionId { get; set; }
    public Guid BuffDefinitionId { get; set; }
    public BuffTriggerCondition TriggerCondition { get; set; }
    public float Chance { get; set; }
    public string DurationOverride { get; set; }
    public string ValueOverride { get; set; }
    public string CustomData { get; set; }
}
