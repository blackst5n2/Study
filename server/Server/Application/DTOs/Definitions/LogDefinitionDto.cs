using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class LogDefinitionDto
{
    public Guid Id { get; set; }
    public Guid EntityDefinitionId { get; set; }
    public ItemGrade Grade { get; set; }
    public ItemSubType RequiredToolSubtype { get; set; }
    public int RequiredToolLevel { get; set; }
    public int RequiredSkillLevel { get; set; }
    public int LoggingDifficulty { get; set; }
    public string RespawnTimeSeconds { get; set; }
    public string CustomData { get; set; }
}
