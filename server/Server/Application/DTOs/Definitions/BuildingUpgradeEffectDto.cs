using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class BuildingUpgradeEffectDto
{
    public Guid Id { get; set; }
    public Guid UpgradeDefinitionId { get; set; }
    public BuildingEffectType EffectType { get; set; }
    public string EffectKey { get; set; }
    public string Value { get; set; }
    public string ExtraData { get; set; }
}
