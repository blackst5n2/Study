namespace Server.Application.DTOs.Definitions;

public class LevelDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public int Level { get; set; }
    public string ExpRequired { get; set; }
}
