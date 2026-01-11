namespace Server.Application.DTOs.Entities;

public class DungeonRunParticipantDto
{
    public Guid Id { get; set; }
    public Guid DungeonRunId { get; set; }
    public Guid PlayerId { get; set; }
    public DateTime JoinedAt { get; set; }
    public string LeftAt { get; set; }
    public bool IsCleared { get; set; }
    public string CustomData { get; set; }
}
