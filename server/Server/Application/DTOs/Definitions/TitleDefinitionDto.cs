namespace Server.Application.DTOs.Definitions;

public class TitleDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public Guid? CategoryId { get; set; }
    public string CustomData { get; set; }
}
