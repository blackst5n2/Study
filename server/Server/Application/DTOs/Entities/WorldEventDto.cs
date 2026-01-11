using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class WorldEventDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public WorldEventType EventType { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public bool IsActive { get; set; }
    public string TriggerCondition { get; set; }
    public string EffectData { get; set; }
    public Guid? MapId { get; set; }
    public string CustomData { get; set; }
}
