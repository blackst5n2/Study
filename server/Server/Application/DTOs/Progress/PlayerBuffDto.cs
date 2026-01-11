using Server.Enums;
namespace Server.Application.DTOs.Progress;

public class PlayerBuffDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid BuffDefinitionId { get; set; }
    public BuffSourceType SourceType { get; set; }
    public Guid? SourceId { get; set; }
    public DateTime StartedAt { get; set; }
    public string ExpiresAt { get; set; }
    public int StackCount { get; set; }
    public string CustomData { get; set; }
}
