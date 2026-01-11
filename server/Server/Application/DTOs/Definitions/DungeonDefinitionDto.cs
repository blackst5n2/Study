namespace Server.Application.DTOs.Definitions;

public class DungeonDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid? EntryPortalId { get; set; }
    public int MinPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public int LevelRequirement { get; set; }
    public string RewardPreview { get; set; }
    public bool IsRandom { get; set; }
    public bool HasBoss { get; set; }
    public string TimeLimitSeconds { get; set; }
    public string CustomData { get; set; }
}
