namespace Server.Application.DTOs.Progress;

public class NpcInstanceDto
{
    public Guid Id { get; set; }
    public Guid NpcDefinitionId { get; set; }
    public Guid MapId { get; set; }
    public Guid? ZoneId { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
    public float RotationY { get; set; }
    public string SpawnCondition { get; set; }
    public string DespawnCondition { get; set; }
    public string CurrentBehaviorState { get; set; }
    public bool IsActive { get; set; }
    public string CustomData { get; set; }
}
