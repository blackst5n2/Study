using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class QuestRewardDto
{
    public Guid Id { get; set; }
    public Guid QuestId { get; set; }
    public QuestRewardType Type { get; set; }
    public string TargetId { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
    public string CustomData { get; set; }
}
