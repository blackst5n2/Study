namespace Server.Application.DTOs.Entities;

public class QuestStepDto
{
    public Guid Id { get; set; }
    public Guid QuestId { get; set; }
    public int StepOrder { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CustomData { get; set; }
}
