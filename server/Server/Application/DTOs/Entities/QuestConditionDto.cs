using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class QuestConditionDto
{
    public Guid Id { get; set; }
    public Guid QuestId { get; set; }
    public int Sequence { get; set; }
    public QuestConditionType Type { get; set; }
    public string TargetId { get; set; }
    public int RequiredValue { get; set; }
    public string Description { get; set; }
    public string CustomData { get; set; }
}
