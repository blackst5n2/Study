namespace Server.Application.DTOs.Progress;

public class ItemInstanceDto
{
    public Guid Id { get; set; }
    public Guid ContainerId { get; set; }
    public Guid ItemDefinitionId { get; set; }
    public int Quantity { get; set; }
    public string SlotIndex { get; set; }
    public string Durability { get; set; }
    public string MaxDurability { get; set; }
    public int EnhancementLevel { get; set; }
    public DateTime AcquiredAt { get; set; }
    public string ExpiresAt { get; set; }
    public bool IsLocked { get; set; }
    public string CustomData { get; set; }
}
