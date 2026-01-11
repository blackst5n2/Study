namespace Server.Application.DTOs.Progress;

public class PlayerEventProgressDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid PlayerId { get; set; }
}
