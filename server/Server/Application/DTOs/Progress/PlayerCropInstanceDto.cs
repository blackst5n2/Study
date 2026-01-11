using Server.Enums;
namespace Server.Application.DTOs.Progress;

public class PlayerCropInstanceDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid CropDefinitionId { get; set; }
    public Guid? FarmPlotId { get; set; }
    public Guid? LandPlotId { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
    public DateTime PlantTime { get; set; }
    public int GrowthStage { get; set; }
    public float CurrentGrowthTimeMinutes { get; set; }
    public bool IsWatered { get; set; }
    public string LastWateredAt { get; set; }
    public string DiseaseState { get; set; }
    public string ExpectedHarvestTime { get; set; }
    public string ActualHarvestTime { get; set; }
    public string AppliedFertilizer { get; set; }
    public CropGrade CurrentGrade { get; set; }
    public string CustomData { get; set; }
}
