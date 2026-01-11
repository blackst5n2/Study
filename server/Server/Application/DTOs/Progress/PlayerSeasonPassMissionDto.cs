namespace Server.Application.DTOs.Progress;

public class PlayerSeasonPassMissionDto
{
    public Guid Id { get; set; }
    public Guid PlayerSeasonPassId { get; set; }
    public Guid SeasonPassMissionId { get; set; }
    public int Progress { get; set; }
    public string LastProgressAt { get; set; }
    public string CompletedAt { get; set; }
    public string ClaimedAt { get; set; }
    public string CustomData { get; set; }
}
