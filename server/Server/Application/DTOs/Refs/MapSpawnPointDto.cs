namespace Server.Application.DTOs.Refs;

public class MapSpawnPointDto
{
    public Guid Id { get; set; }
    public Guid MapId { get; set; }
    public Guid? EntityDefinitionId { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
    public float SpawnRadius { get; set; }
    public string InitialDirection { get; set; }
    public int MaxConcurrent { get; set; }
    public string RespawnTimeSeconds { get; set; }
    public string SpawnCondition { get; set; }
    public string CustomData { get; set; }
}
