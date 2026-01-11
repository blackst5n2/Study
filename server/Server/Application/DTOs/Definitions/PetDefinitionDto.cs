using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class PetDefinitionDto
{
    public Guid Id { get; set; }
    public Guid EntityDefinitionId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Species { get; set; }
    public PetRarity Rarity { get; set; }
    public PetType Type { get; set; }
    public string BaseStats { get; set; }
    public string SkillSet { get; set; }
    public string Description { get; set; }
    public Guid? EvolveTo { get; set; }
    public string Icon { get; set; }
    public string ModelAsset { get; set; }
    public string CustomData { get; set; }
}
