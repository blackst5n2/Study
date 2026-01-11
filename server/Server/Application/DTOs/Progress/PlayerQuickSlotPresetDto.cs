namespace Server.Application.DTOs.Progress;

public class PlayerQuickSlotPresetDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public string Name { get; set; }
    public int PresetIndex { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CustomData { get; set; }
}
