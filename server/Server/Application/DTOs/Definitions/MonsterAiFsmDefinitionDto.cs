namespace Server.Application.DTOs.Definitions;

public class MonsterAiFsmDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string FsmJson { get; set; }
}
