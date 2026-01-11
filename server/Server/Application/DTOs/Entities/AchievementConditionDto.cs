using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class AchievementConditionDto
{
    public Guid Id { get; set; }
    public Guid AchievementId { get; set; }
    public int Sequence { get; set; }
    public AchievementConditionType Type { get; set; }
    public string TargetId { get; set; }
    public string RequiredValue { get; set; }
    public string Description { get; set; }
    public string CustomData { get; set; }
}
