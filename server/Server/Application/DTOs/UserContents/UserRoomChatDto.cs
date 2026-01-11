namespace Server.Application.DTOs.UserContents;

public class UserRoomChatDto
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Guid PlayerId { get; set; }
    public string Message { get; set; }
    public DateTime SentAt { get; set; }
}
