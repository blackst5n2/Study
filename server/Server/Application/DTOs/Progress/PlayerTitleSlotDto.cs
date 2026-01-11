namespace Server.Application.DTOs.Progress;

public class PlayerTitleSlotDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public int SlotIndex { get; set; }
    public Guid? PlayerTitleId { get; set; }
    public string EquippedAt { get; set; }
}
