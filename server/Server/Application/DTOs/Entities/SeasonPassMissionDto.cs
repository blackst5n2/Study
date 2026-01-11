using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class SeasonPassMissionDto
{
    public Guid Id { get; set; }
    public Guid SeasonPassId { get; set; }
    public string MissionCode { get; set; }
    public string Description { get; set; }
    public SeasonPassMissionType Type { get; set; }
    public string Group { get; set; }
    public bool Repeatable { get; set; }
    public int Goal { get; set; }
    public int RewardExp { get; set; }
    public int OrderIndex { get; set; }
}
