namespace Server.Application.DTOs.Entities;

public class ClassTreeDto
{
    public Guid Id { get; set; }
    public Guid? ParentClassId { get; set; }
    public Guid ChildClassId { get; set; }
    public string UnlockCondition { get; set; }
    public string CustomData { get; set; }
}
