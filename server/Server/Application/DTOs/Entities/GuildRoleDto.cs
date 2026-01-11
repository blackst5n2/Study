namespace Server.Application.DTOs.Entities;

public class GuildRoleDto
{
    public Guid Id { get; set; }
    public Guid GuildId { get; set; }
    public string Name { get; set; }
    public bool IsLeaderRole { get; set; }
    public string Permissions { get; set; }
    public int RoleOrder { get; set; }
    public string CustomData { get; set; }
}
