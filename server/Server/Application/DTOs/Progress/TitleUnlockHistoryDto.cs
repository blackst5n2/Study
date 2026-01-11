using Server.Enums;
namespace Server.Application.DTOs.Progress;

public class TitleUnlockHistoryDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid TitleId { get; set; }
    public TitleUnlockEvent Event { get; set; }
    public DateTime OccurredAt { get; set; }
    public string Reason { get; set; }
    public string CustomData { get; set; }
}
