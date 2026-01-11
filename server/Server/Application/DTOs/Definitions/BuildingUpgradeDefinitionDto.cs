namespace Server.Application.DTOs.Definitions;

public class BuildingUpgradeDefinitionDto
{
    public Guid Id { get; set; }
    public Guid BuildingDefinitionId { get; set; }
    public int Level { get; set; }
    public string UpgradeEffectDescription { get; set; }
    public int UpgradeTimeSeconds { get; set; }
}
