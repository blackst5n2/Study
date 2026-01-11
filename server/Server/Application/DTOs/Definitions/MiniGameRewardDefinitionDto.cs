using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class MiniGameRewardDefinitionDto
{
    public Guid Id { get; set; }
    public Guid MinigameId { get; set; }
    public MiniGameResultGrade ResultGrade { get; set; }
    public Guid RewardItemId { get; set; }
    public int MinQuantity { get; set; }
    public int MaxQuantity { get; set; }
    public float Probability { get; set; }
    public string CustomData { get; set; }
}
