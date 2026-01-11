namespace Server.Application.DTOs.Refs;

public class MapDynamicObjectStateDto
{
    public Guid Id { get; set; }
    public Guid MapId { get; set; }
    public Guid ObjectInstanceId { get; set; }
    public string State { get; set; }
    public string Health { get; set; }
    public string GrowthStage { get; set; }
    public string GrowthTimer { get; set; }
    public DateTime LastChangedAt { get; set; }
    public Guid? OwnerId { get; set; }
    public string CustomData { get; set; }
}
