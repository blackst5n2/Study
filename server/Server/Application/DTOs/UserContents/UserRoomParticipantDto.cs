namespace Server.Application.DTOs.UserContents;

public class UserRoomParticipantDto
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Guid PlayerId { get; set; }
    public string Role { get; set; }
    public DateTime JoinedAt { get; set; }
    public string LeftAt { get; set; }
    public string CustomData { get; set; }
}
