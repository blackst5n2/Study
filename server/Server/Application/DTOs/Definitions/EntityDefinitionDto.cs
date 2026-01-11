using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class EntityDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public EntityType EntityType { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid? DropTableId { get; set; }
    public ResourceType ResourceType { get; set; }
    public string ModelAsset { get; set; }
    public string Icon { get; set; }
    public string CustomData { get; set; }
}
