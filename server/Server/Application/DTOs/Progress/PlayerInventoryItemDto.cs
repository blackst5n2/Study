using Server.Enums;
namespace Server.Application.DTOs.Progress;

public class PlayerInventoryItemDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid ItemInstanceId { get; set; }
    public InventoryType InventoryType { get; set; }
    public int SlotIndex { get; set; }
    public InventoryItemStatus Status { get; set; }
    public string CustomData { get; set; }
}
