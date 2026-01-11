using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class TitleUnlockConditionDto
{
    public Guid Id { get; set; }
    public Guid TitleId { get; set; }
    public int ConditionGroup { get; set; }
    public TitleUnlockType Type { get; set; }
    public string Key { get; set; }
    public TitleUnlockOperator Operator { get; set; }
    public string Value { get; set; }
    public string Extra { get; set; }
}
