namespace Server.Application.DTOs.Entities;

public class PetSkillDto
{
    public Guid Id { get; set; }
    public Guid PetDefinitionId { get; set; }
    public Guid SkillDefinitionId { get; set; }
    public int UnlockLevel { get; set; }
    public bool IsPassive { get; set; }
    public string CustomData { get; set; }
}
