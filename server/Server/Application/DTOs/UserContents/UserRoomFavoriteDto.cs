namespace Server.Application.DTOs.UserContents;

public class UserRoomFavoriteDto
{
    public Guid PlayerId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CreatedAt { get; set; }
}
