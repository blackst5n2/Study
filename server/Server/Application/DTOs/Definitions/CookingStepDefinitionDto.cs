using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class CookingStepDefinitionDto
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public int StepIndex { get; set; }
    public CookingAction Action { get; set; }
    public ItemSubType RequiredTool { get; set; }
    public string RequiredTimeSec { get; set; }
    public string Temperature { get; set; }
    public string Description { get; set; }
    public string CustomData { get; set; }
}
