using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class StatValueDto
{
    public Guid Id { get; set; }
    public StatOwnerType OwnerType { get; set; }
    public Guid OwnerId { get; set; }
    public Guid StatDefinitionId { get; set; }
    public string Value { get; set; }
}
