using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class NotificationDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public GameNotificationType Type { get; set; }
    public string Payload { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ExpiresAt { get; set; }
}
