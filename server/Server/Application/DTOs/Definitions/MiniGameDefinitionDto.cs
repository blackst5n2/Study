using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class MiniGameDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public MiniGameType Type { get; set; }
    public string Description { get; set; }
    public string Rules { get; set; }
    public string CustomData { get; set; }
}
