namespace Server.Application.DTOs.Entities;

public class BoardGameRoomDto
{
    public Guid Id { get; set; }
    public Guid BoardGameId { get; set; }
    public string RoomName { get; set; }
    public Guid HostPlayerId { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string StartedAt { get; set; }
    public string EndedAt { get; set; }
    public Guid? WinnerPlayerId { get; set; }
    public string CustomData { get; set; }
}
