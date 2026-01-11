using Server.Enums;
namespace Server.Application.DTOs.Refs;

public class SeedGradeToCropGradeMapDto
{
    public Guid Id { get; set; }
    public Guid SeedItemId { get; set; }
    public ItemGrade SeedGrade { get; set; }
    public Guid CropDefinitionId { get; set; }
    public Guid? FertilizerItemId { get; set; }
    public string EnvironmentCondition { get; set; }
    public CropGrade CropGrade { get; set; }
    public float Probability { get; set; }
    public string CustomData { get; set; }
}
