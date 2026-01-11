using Server.Enums;
namespace Server.Application.DTOs.Progress;

public class RankingHistoryDto
{
    public Guid Id { get; set; }
    public RankingCategory Category { get; set; }
    public RankingTargetType TargetType { get; set; }
    public Guid TargetId { get; set; }
    public string Score { get; set; }
    public int Rank { get; set; }
    public RankingSeason Season { get; set; }
    public DateTime RecordedAt { get; set; }
}
