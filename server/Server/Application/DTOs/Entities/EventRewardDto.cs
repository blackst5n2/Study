namespace Server.Application.DTOs.Entities;

public class EventRewardDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string RewardCode { get; set; }
}
