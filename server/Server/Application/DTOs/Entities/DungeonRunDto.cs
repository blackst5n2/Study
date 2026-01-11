using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class DungeonRunDto
{
    public Guid Id { get; set; }
    public Guid DungeonId { get; set; }
    public Guid? PartyId { get; set; }
    public Guid LeaderId { get; set; }
    public DungeonRunStatus Status { get; set; }
    public DateTime StartedAt { get; set; }
    public string EndedAt { get; set; }
    public Guid? CurrentZoneId { get; set; }
    public float ElapsedTimeSeconds { get; set; }
    public string CustomData { get; set; }
}
