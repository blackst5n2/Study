using Server.Enums;
namespace Server.Application.DTOs.Refs;

public class MapZoneDto
{
    public Guid Id { get; set; }
    public Guid MapId { get; set; }
    public string Name { get; set; }
    public MapZoneType ZoneType { get; set; }
    public MapZoneAreaType AreaType { get; set; }
    public string AreaData { get; set; }
    public int Priority { get; set; }
    public Guid? EnvironmentId { get; set; }
    public string AllowPvp { get; set; }
    public string AllowFarming { get; set; }
    public string AllowFishing { get; set; }
    public string AllowBuilding { get; set; }
    public string EntryCondition { get; set; }
    public bool Active { get; set; }
    public string CustomData { get; set; }
}
