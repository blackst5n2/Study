using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class GuildJoinRequestDto
{
    public Guid Id { get; set; }
    public Guid GuildId { get; set; }
    public Guid PlayerId { get; set; }
    public GuildJoinRequestStatus Status { get; set; }
    public string Message { get; set; }
    public DateTime RequestedAt { get; set; }
    public string ProcessedAt { get; set; }
    public Guid? ProcessedBy { get; set; }
    public string Reason { get; set; }
    public string CustomData { get; set; }
}
