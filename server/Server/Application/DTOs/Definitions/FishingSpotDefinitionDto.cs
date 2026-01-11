using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class FishingSpotDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public Guid MapId { get; set; }
    public Guid? ZoneId { get; set; }
    public MapZoneAreaType AreaType { get; set; }
    public string AreaData { get; set; }
    public int RequiredFishingLevel { get; set; }
    public string CustomData { get; set; }
}
