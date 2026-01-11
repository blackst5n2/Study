using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class MapDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public MapType Type { get; set; }
    public ViewMode ViewMode { get; set; }
    public float SizeX { get; set; }
    public float SizeY { get; set; }
    public string BackgroundAsset { get; set; }
    public bool IsSafeZone { get; set; }
    public Guid? ParentMapId { get; set; }
    public string MinLevel { get; set; }
    public string MaxLevel { get; set; }
    public string CustomData { get; set; }
}
