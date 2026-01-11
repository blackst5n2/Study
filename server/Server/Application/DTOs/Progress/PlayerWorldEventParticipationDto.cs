namespace Server.Application.DTOs.Progress;

public class PlayerWorldEventParticipationDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid EventId { get; set; }
    public DateTime JoinedAt { get; set; }
    public string Progress { get; set; }
    public bool RewardClaimed { get; set; }
    public string CustomData { get; set; }
}
