using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class RecipeDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public RecipeType Type { get; set; }
    public string RequiredBuildingCode { get; set; }
    public int RequiredLevel { get; set; }
    public float SuccessRate { get; set; }
    public int CraftTimeSeconds { get; set; }
    public string UnlockCondition { get; set; }
    public bool IsRepeatable { get; set; }
    public string PityThreshold { get; set; }
    public string PitySuccessRate { get; set; }
    public Guid? FailRewardItemId { get; set; }
    public string FailRewardQuantity { get; set; }
    public string CustomData { get; set; }
}
