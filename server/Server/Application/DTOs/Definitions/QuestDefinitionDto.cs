namespace Server.Application.DTOs.Definitions;

public class QuestDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid? GroupId { get; set; }
    public int RequiredLevel { get; set; }
    public bool Repeatable { get; set; }
    public string CooldownMinutes { get; set; }
    public bool IsStoryQuest { get; set; }
    public bool IsDaily { get; set; }
    public bool IsWeekly { get; set; }
    public string StartNpcCode { get; set; }
    public string EndNpcCode { get; set; }
    public bool AutoAccept { get; set; }
    public bool AutoComplete { get; set; }
    public string CustomData { get; set; }
}
