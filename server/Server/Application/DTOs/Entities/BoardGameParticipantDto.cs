namespace Server.Application.DTOs.Entities;

public class BoardGameParticipantDto
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public Guid PlayerId { get; set; }
    public DateTime JoinTime { get; set; }
    public string LeaveTime { get; set; }
    public string SeatNo { get; set; }
    public string IsWinner { get; set; }
    public string Score { get; set; }
    public string CustomData { get; set; }
}
