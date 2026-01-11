namespace Server.Application.DTOs.Entities;

public class GuildMemberDto
{
    public Guid Id { get; set; }
    public Guid GuildId { get; set; }
    public Guid PlayerId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime JoinedAt { get; set; }
    public string LeftAt { get; set; }
    public bool IsActive { get; set; }
    public int ContributionPoints { get; set; }
    public string LastActivityAt { get; set; }
    public string CustomData { get; set; }
}
