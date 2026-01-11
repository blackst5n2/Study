namespace Server.Application.DTOs.Definitions;

public class SeasonPassDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int Priority { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public bool IsActive { get; set; }
    public bool FreeTrack { get; set; }
    public bool PremiumTrack { get; set; }
    public string MaxLevel { get; set; }
    public string PremiumProductId { get; set; }
    public string LevelSkipProductId { get; set; }
    public string Config { get; set; }
    public string CustomData { get; set; }
}
