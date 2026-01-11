using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class AchievementRewardDto
{
    public Guid Id { get; set; }
    public Guid AchievementId { get; set; }
    public AchievementRewardType Type { get; set; }
    public string TargetId { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
    public string CustomData { get; set; }
}
