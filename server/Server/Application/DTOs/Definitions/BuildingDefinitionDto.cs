using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class BuildingDefinitionDto
{
    public Guid Id { get; set; }
    public Guid EntityDefinitionId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public BuildingType Type { get; set; }
    public string Description { get; set; }
    public string AllowedActions { get; set; }
    public string UnlockCondition { get; set; }
    public int MaxLevel { get; set; }
    public string PlacementRules { get; set; }
    public string Icon { get; set; }
    public string ModelAsset { get; set; }
    public string CustomData { get; set; }
}
