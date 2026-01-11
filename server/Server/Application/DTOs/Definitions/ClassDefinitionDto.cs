using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class ClassDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid? ParentClassId { get; set; }
    public string UnlockCondition { get; set; }
    public bool IsBase { get; set; }
    public bool IsActive { get; set; }
    public string Icon { get; set; }
    public ClassRole Role { get; set; }
    public string CustomData { get; set; }
}
