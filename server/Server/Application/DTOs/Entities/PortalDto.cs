using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class PortalDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public Guid FromMapId { get; set; }
    public float FromPosX { get; set; }
    public float FromPosY { get; set; }
    public float FromPosZ { get; set; }
    public float FromRadius { get; set; }
    public Guid ToMapId { get; set; }
    public float ToPosX { get; set; }
    public float ToPosY { get; set; }
    public float ToPosZ { get; set; }
    public string ToDirection { get; set; }
    public PortalType PortalType { get; set; }
    public bool IsActive { get; set; }
    public bool IsBidirectional { get; set; }
    public string RequiredLevel { get; set; }
    public string RequiredQuestCode { get; set; }
    public string RequiredItemCode { get; set; }
    public bool RequiredItemConsumed { get; set; }
    public string EntryCondition { get; set; }
    public string CustomData { get; set; }
}
