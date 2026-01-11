using Server.Enums;
namespace Server.Application.DTOs.Progress;

public class PlayerBuildingDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid LandPlotId { get; set; }
    public Guid BuildingDefinitionId { get; set; }
    public Guid? InventoryItemId { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
    public float RotationY { get; set; }
    public DateTime PlacedAt { get; set; }
    public string RemovedAt { get; set; }
    public int Level { get; set; }
    public string CurrentUpgradeFinishTime { get; set; }
    public string Durability { get; set; }
    public BuildingStatus Status { get; set; }
    public string CustomData { get; set; }
}
