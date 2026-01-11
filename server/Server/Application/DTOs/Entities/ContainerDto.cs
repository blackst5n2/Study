using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class ContainerDto
{
    public Guid Id { get; set; }
    public ContainerType ContainerType { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; }
    public int SlotCount { get; set; }
    public string CustomData { get; set; }
}
