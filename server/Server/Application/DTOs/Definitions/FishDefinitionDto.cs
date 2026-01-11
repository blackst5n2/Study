using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class FishDefinitionDto
{
    public Guid Id { get; set; }
    public Guid EntityDefinitionId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public ItemGrade Grade { get; set; }
    public int RequiredFishingLevel { get; set; }
    public Guid ItemDefinitionId { get; set; }
    public string MinLengthCm { get; set; }
    public string MaxLengthCm { get; set; }
    public string CustomData { get; set; }
}
