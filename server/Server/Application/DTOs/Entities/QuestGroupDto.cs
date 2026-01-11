namespace Server.Application.DTOs.Entities;

public class QuestGroupDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CustomData { get; set; }
}
