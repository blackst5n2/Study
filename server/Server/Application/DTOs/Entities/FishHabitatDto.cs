using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class FishHabitatDto
{
    public Guid Id { get; set; }
    public Guid FishDefinitionId { get; set; }
    public Guid FishingSpotId { get; set; }
    public float SpawnChance { get; set; }
    public Guid? RequiredBaitItemId { get; set; }
    public TimeOfDayType RequiredTime { get; set; }
    public SeasonType RequiredSeason { get; set; }
    public WeatherType RequiredWeather { get; set; }
    public string CustomData { get; set; }
}
