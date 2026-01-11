using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class ClassEquipmentRestrictionDto
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public ItemType ItemType { get; set; }
    public ItemSubType ItemSubType { get; set; }
    public Guid? ItemTagId { get; set; }
    public RestrictionType RestrictionType { get; set; }
    public string CustomData { get; set; }
}
