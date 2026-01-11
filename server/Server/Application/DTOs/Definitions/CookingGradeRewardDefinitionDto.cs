using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class CookingGradeRewardDefinitionDto
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public CookingGrade Grade { get; set; }
    public Guid RewardItemId { get; set; }
    public int Quantity { get; set; }
    public float Probability { get; set; }
    public string CustomData { get; set; }
}
