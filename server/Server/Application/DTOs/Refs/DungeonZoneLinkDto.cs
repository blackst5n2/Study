using Server.Enums;
namespace Server.Application.DTOs.Refs;

public class DungeonZoneLinkDto
{
    public Guid Id { get; set; }
    public Guid DungeonId { get; set; }
    public Guid MapZoneId { get; set; }
    public int Sequence { get; set; }
    public DungeonZoneType ZoneType { get; set; }
    public bool IsStartZone { get; set; }
    public bool IsEndZone { get; set; }
    public string CustomData { get; set; }
}
