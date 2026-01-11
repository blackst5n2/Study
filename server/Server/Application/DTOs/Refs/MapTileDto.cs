namespace Server.Application.DTOs.Refs;

public class MapTileDto
{
    public Guid Id { get; set; }
    public Guid LayerId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public string TileAsset { get; set; }
    public string CustomData { get; set; }
}
