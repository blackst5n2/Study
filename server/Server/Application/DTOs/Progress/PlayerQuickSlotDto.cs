using Server.Enums;
namespace Server.Application.DTOs.Progress;

public class PlayerQuickSlotDto
{
    public Guid Id { get; set; }
    public Guid PresetId { get; set; }
    public int SlotIndex { get; set; }
    public QuickSlotType SlotType { get; set; }
    public Guid? SkillDefinitionId { get; set; }
    public Guid? ItemDefinitionId { get; set; }
    public string Hotkey { get; set; }
    public string CustomData { get; set; }
}
