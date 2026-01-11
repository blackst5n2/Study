using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class EditableAreaDto
{
    public Guid Id { get; set; }
    public Guid MapId { get; set; }
    public MapZoneAreaType AreaType { get; set; }
    public string AreaData { get; set; }
    public bool IsBuildable { get; set; }
    public bool IsFarmable { get; set; }
    public string UnlockCondition { get; set; }
    public LandOwnershipType OwnerType { get; set; }
    public Guid? OwnerId { get; set; }
    public string CustomData { get; set; }
}
