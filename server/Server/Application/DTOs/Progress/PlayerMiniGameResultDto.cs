using Server.Enums;
namespace Server.Application.DTOs.Progress;

public class PlayerMiniGameResultDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid MinigameId { get; set; }
    public DateTime PlayedAt { get; set; }
    public string Score { get; set; }
    public string ComboCount { get; set; }
    public string TapCount { get; set; }
    public string PatternAnswered { get; set; }
    public string BonusTimeSuccess { get; set; }
    public string GoldenButtonSuccess { get; set; }
    public MiniGameResultGrade ResultGrade { get; set; }
    public Guid? RewardItemId { get; set; }
    public string RewardQuantity { get; set; }
    public string CustomData { get; set; }
}
