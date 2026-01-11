using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class FriendRelationDto
{
    public Guid Id { get; set; }
    public Guid PlayerId1 { get; set; }
    public Guid PlayerId2 { get; set; }
    public FriendStatus Status { get; set; }
    public string RequestedAt { get; set; }
    public string AcceptedAt { get; set; }
    public string BlockedAt { get; set; }
    public string LastOnlineAt { get; set; }
    public string CustomData { get; set; }
}
