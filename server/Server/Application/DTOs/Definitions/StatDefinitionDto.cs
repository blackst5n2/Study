using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class StatDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public StatType StatType { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsPercentage { get; set; }
    public string DefaultValue { get; set; }
    public string MinValue { get; set; }
    public string MaxValue { get; set; }
    public string CustomData { get; set; }
}
