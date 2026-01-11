using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class ItemDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ItemType Type { get; set; }
    public ItemSubType SubType { get; set; }
    public ItemGrade Grade { get; set; }
    public ItemRarity Rarity { get; set; }
    public EquipSlot EquipSlot { get; set; }
    public int MaxStack { get; set; }
    public string BaseValue { get; set; }
    public string Price { get; set; }
    public bool IsTradable { get; set; }
    public bool IsUsable { get; set; }
    public Guid? UseEffectId { get; set; }
    public string RequiredLevel { get; set; }
    public string Icon { get; set; }
    public string ModelAsset { get; set; }
    public string CustomData { get; set; }
}
