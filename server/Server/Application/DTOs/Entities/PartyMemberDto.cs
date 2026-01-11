using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class PartyMemberDto
{
    public Guid Id { get; set; }
    public Guid PartyId { get; set; }
    public Guid PlayerId { get; set; }
    public DateTime JoinedAt { get; set; }
    public string LeftAt { get; set; }
    public PartyRole Role { get; set; }
    public bool IsActive { get; set; }
    public string CustomData { get; set; }
}
