namespace Server.Application.DTOs.Entities;

public class GuildTitleDto
{
    public Guid Id { get; set; }
    public Guid GuildId { get; set; }
    public Guid TitleId { get; set; }
    public DateTime AcquiredAt { get; set; }
    public bool IsActive { get; set; }
    public string CustomData { get; set; }
}
