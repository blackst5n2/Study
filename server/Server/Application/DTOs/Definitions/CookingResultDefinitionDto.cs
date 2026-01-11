using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class CookingResultDefinitionDto
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public Doneness Doneness { get; set; }
    public string TemperatureRange { get; set; }
    public Guid ResultItemId { get; set; }
    public int ResultQuantity { get; set; }
    public float Probability { get; set; }
    public string BonusEffect { get; set; }
    public string CustomData { get; set; }
}
