using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class ChannelDefinitionDto
{
    public string Code { get; set; }
    public string Name { get; set; }
    public ChannelType Type { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public string CustomData { get; set; }
}
