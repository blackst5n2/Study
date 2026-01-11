namespace Server.Application.DTOs.Definitions;

public class GuildDefinitionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Tag { get; set; }
    public string Description { get; set; }
    public Guid LeaderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public int MemberLimit { get; set; }
    public bool IsActive { get; set; }
    public bool IsRecruiting { get; set; }
    public string CustomData { get; set; }
}
