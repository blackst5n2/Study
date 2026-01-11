using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class JobDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string UnlockCondition { get; set; }
    public string Icon { get; set; }
    public JobType JobType { get; set; }
    public bool IsPlayable { get; set; }
    public bool IsHidden { get; set; }
    public int OrderIndex { get; set; }
    public string CustomData { get; set; }
}
