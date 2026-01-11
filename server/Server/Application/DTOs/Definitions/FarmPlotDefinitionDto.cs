namespace Server.Application.DTOs.Definitions;

public class FarmPlotDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public Guid? MapId { get; set; }
    public Guid? BuildingDefinitionId { get; set; }
    public string AllowedCropTags { get; set; }
    public string AllowedFertilizerTags { get; set; }
    public string CustomData { get; set; }
}
