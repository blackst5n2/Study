namespace Server.Application.DTOs.Progress;

public class PlayerDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string Code { get; set; }
    public string Nickname { get; set; }
    public int Level { get; set; }
    public string Experience { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLogoutAt { get; set; }
    public int PlayTimeSeconds { get; set; }
    public string CustomData { get; set; }
}
