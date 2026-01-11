using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class CurrencyDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public CurrencyType CurrencyType { get; set; }
    public string Description { get; set; }
    public bool IsPremium { get; set; }
    public string Icon { get; set; }
    public string CustomData { get; set; }
}
