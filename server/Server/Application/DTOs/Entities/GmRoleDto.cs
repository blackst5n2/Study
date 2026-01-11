using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class GmRoleDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public GmRoleType RoleType { get; set; }
    public string Permissions { get; set; }
    public string CustomData { get; set; }
}
