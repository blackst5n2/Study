namespace Server.Application.DTOs.Definitions;

public class CropProductDto
{
    public Guid Id { get; set; }
    public Guid CropDefinitionId { get; set; }
    public Guid ItemDefinitionId { get; set; }
    public int MinYield { get; set; }
    public int MaxYield { get; set; }
}
