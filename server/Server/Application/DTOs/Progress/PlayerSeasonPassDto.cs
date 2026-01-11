namespace Server.Application.DTOs.Progress;

public class PlayerSeasonPassDto
{
    public Guid Id { get; set; }
    public Guid SeasonPassId { get; set; }
    public Guid PlayerId { get; set; }
    public bool PremiumUnlocked { get; set; }
    public int CurrentLevel { get; set; }
    public int CurrentExp { get; set; }
    public string LastMissionRefreshAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string CustomData { get; set; }
}
