using Server.Enums;
namespace Server.Application.DTOs.Refs;

public class MapEventTriggerDto
{
    public Guid Id { get; set; }
    public Guid MapId { get; set; }
    public WorldEventType EventType { get; set; }
    public string PosX { get; set; }
    public string PosY { get; set; }
    public string PosZ { get; set; }
    public string TriggerRadius { get; set; }
    public string TriggerCondition { get; set; }
    public string Action { get; set; }
    public string Parameters { get; set; }
    public string CooldownSeconds { get; set; }
    public string CustomData { get; set; }
}
