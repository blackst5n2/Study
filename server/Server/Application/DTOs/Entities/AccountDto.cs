namespace Server.Application.DTOs.Entities;

public class AccountDto
{
    public Guid Id { get; set; }
    public string Nickname { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginAt { get; set; }
    public string Status { get; set; }
    public string CustomData { get; set; }
}
