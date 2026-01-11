using Server.Enums;
namespace Server.Application.DTOs.Progress;

public class PlayerBanDto
{
    public Guid Id { get; set; }
    public Guid? PlayerId { get; set; }
    public Guid? AccountId { get; set; }
    public BanType BanType { get; set; }
    public string Reason { get; set; }
    public DateTime StartAt { get; set; }
    public string EndAt { get; set; }
    public string IssuedBy { get; set; }
    public Guid? RelatedIncidentId { get; set; }
    public bool IsActive { get; set; }
    public string CustomData { get; set; }
}
