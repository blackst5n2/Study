using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class AuthDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public AuthProvider Provider { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string OauthId { get; set; }
    public bool IsVerified { get; set; }
    public string VerifyToken { get; set; }
    public DateTime LastLoginAt { get; set; }
    public string CustomData { get; set; }
}
