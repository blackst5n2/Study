using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class CookingRecipeDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public Guid RecipeDefinitionId { get; set; }
    public string TemperatureRange { get; set; }
    public Doneness OptimalDoneness { get; set; }
    public string BonusEffect { get; set; }
    public string CustomData { get; set; }
}
