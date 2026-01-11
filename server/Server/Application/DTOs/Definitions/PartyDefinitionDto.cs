namespace Server.Application.DTOs.Definitions;

public class PartyDefinitionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid LeaderId { get; set; }
    public string LootRule { get; set; }
    public DateTime CreatedAt { get; set; }
    public string DisbandedAt { get; set; }
    public string CustomData { get; set; }
}
