namespace Server.Application.DTOs.Entities;

public class ClassSkillTreeDto
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public Guid? ParentSkillId { get; set; }
    public Guid ChildSkillId { get; set; }
    public string UnlockCondition { get; set; }
    public string RequiredClassLevel { get; set; }
    public string RequiredSkillLevel { get; set; }
    public string CustomData { get; set; }
}
