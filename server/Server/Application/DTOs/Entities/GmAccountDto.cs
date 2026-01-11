namespace Server.Application.DTOs.Entities;

public class GmAccountDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid GmRoleId { get; set; }
    public string GmName { get; set; }
    public bool IsActive { get; set; }
    public string LastLoginAt { get; set; }
    public string CustomData { get; set; }
}
