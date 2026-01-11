using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class SeasonPassRewardDto
{
    public Guid Id { get; set; }
    public Guid SeasonPassId { get; set; }
    public string RewardCode { get; set; }
    public SeasonPassRewardTrack Track { get; set; }
    public int Level { get; set; }
    public string RewardType { get; set; }
    public bool IsHidden { get; set; }
    public string Condition { get; set; }
    public Guid? MailAttachmentId { get; set; }
    public string DirectReward { get; set; }
    public string CustomData { get; set; }
}
