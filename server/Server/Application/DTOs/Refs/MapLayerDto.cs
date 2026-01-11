using Server.Enums;
namespace Server.Application.DTOs.Refs;

public class MapLayerDto
{
    public Guid Id { get; set; }
    public Guid MapId { get; set; }
    public string Name { get; set; }
    public int LayerOrder { get; set; }
    public MapLayerType Type { get; set; }
    public bool IsVisible { get; set; }
    public string CustomData { get; set; }
}
