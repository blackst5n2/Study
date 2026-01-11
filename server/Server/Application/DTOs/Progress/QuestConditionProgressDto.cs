namespace Server.Application.DTOs.Progress;

public class QuestConditionProgressDto
{
    public Guid Id { get; set; }
    public Guid QuestLogId { get; set; }
    public Guid QuestConditionId { get; set; }
    public int CurrentValue { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime UpdatedAt { get; set; }
}
