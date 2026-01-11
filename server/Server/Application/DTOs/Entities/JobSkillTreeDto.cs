namespace Server.Application.DTOs.Entities;

public class JobSkillTreeDto
{
    public Guid Id { get; set; }
    public Guid JobId { get; set; }
    public Guid? ParentSkillId { get; set; }
    public Guid ChildSkillId { get; set; }
    public string UnlockCondition { get; set; }
    public string RequiredJobLevel { get; set; }
    public string RequiredSkillLevel { get; set; }
    public string CustomData { get; set; }
}
