namespace Server.Application.DTOs.Progress;

public class PlayerPetDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid PetDefinitionId { get; set; }
    public string Nickname { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public DateTime AcquiredAt { get; set; }
    public bool IsSummoned { get; set; }
    public bool IsLocked { get; set; }
    public string CustomData { get; set; }
}
