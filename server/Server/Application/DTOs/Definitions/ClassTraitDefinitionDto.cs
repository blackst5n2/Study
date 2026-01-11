namespace Server.Application.DTOs.Definitions;

public class ClassTraitDefinitionDto
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public string TraitName { get; set; }
    public string Description { get; set; }
    public string EffectData { get; set; }
}
