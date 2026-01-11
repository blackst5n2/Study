using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class NpcDefinitionDto
{
    public Guid Id { get; set; }
    public Guid EntityDefinitionId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public NpcType NpcType { get; set; }
    public string Description { get; set; }
    public string SpriteAsset { get; set; }
    public string ModelAsset { get; set; }
    public string BehaviorScript { get; set; }
    public string DialogueStartCode { get; set; }
    public Guid? ShopId { get; set; }
    public bool IsQuestGiver { get; set; }
    public string Faction { get; set; }
    public string CustomData { get; set; }
}
