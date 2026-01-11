using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class LivestockDefinitionDto
{
    public Guid Id { get; set; }
    public Guid EntityDefinitionId { get; set; }
    public ItemGrade Grade { get; set; }
    public int GrowthStages { get; set; }
    public int GrowthTimeMinutesPerStage { get; set; }
    public string FeedTag { get; set; }
    public int RequiredNutritionPerDay { get; set; }
    public float DiseaseChance { get; set; }
    public string ProductIntervalMinutes { get; set; }
    public Guid? ProductItemId { get; set; }
    public int MinProductAmount { get; set; }
    public int MaxProductAmount { get; set; }
    public string CustomData { get; set; }
}
