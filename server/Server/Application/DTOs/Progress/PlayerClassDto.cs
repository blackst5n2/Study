namespace Server.Application.DTOs.Progress;

public class PlayerClassDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid ClassId { get; set; }
    public bool IsMain { get; set; }
    public bool IsUnlocked { get; set; }
    public string AcquiredAt { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public string CustomData { get; set; }
}
