using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class CropDefinitionDto
{
    public Guid Id { get; set; }
    public Guid EntityDefinitionId { get; set; }
    public int RequiredFarmingLevel { get; set; }
    public SeasonType AllowedSeason { get; set; }
    public string FertilizerEffect { get; set; }
    public CropGrade BaseGrade { get; set; }
    public int GrowthStages { get; set; }
    public int GrowthTimeMinutesPerStage { get; set; }
    public float DiseaseChance { get; set; }
    public int RequiredNutrient { get; set; }
    public int RequiredMoisture { get; set; }
    public string RequiredTemperature { get; set; }
    public string CustomData { get; set; }
}
