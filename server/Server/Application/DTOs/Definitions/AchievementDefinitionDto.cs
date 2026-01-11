namespace Server.Application.DTOs.Definitions;

public class AchievementDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid? CategoryId { get; set; }
    public int RequiredLevel { get; set; }
    public int Points { get; set; }
    public bool IsHidden { get; set; }
    public string Icon { get; set; }
    public string CustomData { get; set; }
}
