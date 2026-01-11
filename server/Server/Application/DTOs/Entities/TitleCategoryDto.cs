namespace Server.Application.DTOs.Entities;

public class TitleCategoryDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int DisplayOrder { get; set; }
}
