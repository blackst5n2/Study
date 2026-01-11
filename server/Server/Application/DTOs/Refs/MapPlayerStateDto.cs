namespace Server.Application.DTOs.Refs;

public class MapPlayerStateDto
{
    public Guid PlayerId { get; set; }
    public Guid MapId { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
    public string RotationY { get; set; }
    public string State { get; set; }
    public DateTime LastSyncAt { get; set; }
    public string CustomData { get; set; }
}
