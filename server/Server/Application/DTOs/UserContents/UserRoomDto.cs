using Server.Enums;
namespace Server.Application.DTOs.UserContents;

public class UserRoomDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string RoomName { get; set; }
    public string Description { get; set; }
    public UserContentStatus Status { get; set; }
    public int MaxPlayers { get; set; }
    public string Password { get; set; }
    public string MapLayout { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public int LikeCount { get; set; }
    public string CustomData { get; set; }
}
