using Server.Enums;
namespace Server.Application.DTOs.Progress;

public class PlayerLivestockInstanceDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid LivestockDefinitionId { get; set; }
    public string Nickname { get; set; }
    public ItemGrade Grade { get; set; }
    public DateTime AcquiredAt { get; set; }
    public LivestockGrowthStage CurrentGrowthStage { get; set; }
    public float CurrentGrowthTimeMinutes { get; set; }
    public string LastFedAt { get; set; }
    public int CurrentNutrition { get; set; }
    public string LastProductCollectedAt { get; set; }
    public string DiseaseState { get; set; }
    public Guid? RanchBuildingId { get; set; }
    public string CustomData { get; set; }
}
