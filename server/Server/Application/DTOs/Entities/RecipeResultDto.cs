using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class RecipeResultDto
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public RecipeResultType ResultType { get; set; }
    public Guid ItemDefinitionId { get; set; }
    public int MinQuantity { get; set; }
    public int MaxQuantity { get; set; }
    public float Probability { get; set; }
    public ItemGrade Grade { get; set; }
    public bool IsFailReward { get; set; }
    public string BonusEffect { get; set; }
    public string CustomData { get; set; }
}
