namespace Server.Application.DTOs.Progress;

public class PlayerTitleDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid TitleId { get; set; }
    public DateTime AcquiredAt { get; set; }
    public bool IsActive { get; set; }
    public string CustomData { get; set; }
}
