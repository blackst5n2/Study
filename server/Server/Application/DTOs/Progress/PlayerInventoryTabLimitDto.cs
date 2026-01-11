using Server.Enums;
namespace Server.Application.DTOs.Progress;

public class PlayerInventoryTabLimitDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public InventoryType InventoryType { get; set; }
    public int MaxSlots { get; set; }
    public string UpgradedAt { get; set; }
    public string CustomData { get; set; }
}
