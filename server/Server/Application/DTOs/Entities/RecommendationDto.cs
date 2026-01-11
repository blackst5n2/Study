using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class RecommendationDto
{
    public Guid Id { get; set; }
    public RecommendationTargetType TargetType { get; set; }
    public Guid TargetId { get; set; }
    public Guid FromPlayerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CustomData { get; set; }
}
