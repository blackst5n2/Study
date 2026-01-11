using Server.Enums;
namespace Server.Application.DTOs.Progress;

public class PlayerLandDto
{
    public Guid Id { get; set; }
    public LandOwnershipType OwnerType { get; set; }
    public Guid OwnerId { get; set; }
    public Guid LandPlotId { get; set; }
    public DateTime PurchasedAt { get; set; }
    public string ExpiresAt { get; set; }
    public string CustomName { get; set; }
    public bool IsPrimary { get; set; }
    public string CustomData { get; set; }
}
