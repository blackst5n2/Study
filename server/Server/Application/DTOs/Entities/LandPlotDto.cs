using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class LandPlotDto
{
    public Guid Id { get; set; }
    public Guid MapId { get; set; }
    public LandPlotType PlotType { get; set; }
    public int CoordX { get; set; }
    public int CoordY { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool IsPublic { get; set; }
    public string Price { get; set; }
    public string UnlockCondition { get; set; }
    public string CustomData { get; set; }
}
