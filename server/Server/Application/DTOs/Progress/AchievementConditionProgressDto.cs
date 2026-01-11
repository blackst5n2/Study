namespace Server.Application.DTOs.Progress;

public class AchievementConditionProgressDto
{
    public Guid Id { get; set; }
    public Guid AchievementLogId { get; set; }
    public Guid AchievementConditionId { get; set; }
    public string CurrentValue { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime UpdatedAt { get; set; }
}
