namespace Server.Application.DTOs.Refs;

public class MapEnvironmentDto
{
    public Guid Id { get; set; }
    public Guid MapId { get; set; }
    public string TemperatureDefault { get; set; }
    public string TemperatureMin { get; set; }
    public string TemperatureMax { get; set; }
    public string Bgm { get; set; }
    public string Ambience { get; set; }
    public string LightingProfile { get; set; }
    public string CustomData { get; set; }
}
