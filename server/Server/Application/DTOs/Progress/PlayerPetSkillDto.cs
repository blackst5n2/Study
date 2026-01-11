namespace Server.Application.DTOs.Progress;

public class PlayerPetSkillDto
{
    public Guid Id { get; set; }
    public Guid PlayerPetId { get; set; }
    public Guid PetSkillId { get; set; }
    public int Level { get; set; }
    public DateTime AcquiredAt { get; set; }
    public bool IsActive { get; set; }
    public string CustomData { get; set; }
}
