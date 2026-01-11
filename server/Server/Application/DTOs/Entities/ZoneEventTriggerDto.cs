using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class ZoneEventTriggerDto
{
    public Guid Id { get; set; }
    public Guid ZoneId { get; set; }
    public WorldEventType EventType { get; set; }
    public string TriggerCondition { get; set; }
    public string Action { get; set; }
    public string Parameters { get; set; }
    public int Priority { get; set; }
    public string CooldownSeconds { get; set; }
    public string MaxTriggers { get; set; }
    public string CustomData { get; set; }
}
