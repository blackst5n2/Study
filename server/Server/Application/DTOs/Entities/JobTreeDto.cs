namespace Server.Application.DTOs.Entities;

public class JobTreeDto
{
    public Guid Id { get; set; }
    public Guid? ParentJobId { get; set; }
    public Guid ChildJobId { get; set; }
    public string UnlockCondition { get; set; }
    public string CustomData { get; set; }
}
