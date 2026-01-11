using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class MonsterDefinitionDto
{
    public Guid Id { get; set; }
    public Guid EntityDefinitionId { get; set; }
    public int Level { get; set; }
    public MonsterType Type { get; set; }
    public int ExpReward { get; set; }
    public Guid? FsmId { get; set; }
    public Guid? BtId { get; set; }
    public string Description { get; set; }
    public string CustomData { get; set; }
}
