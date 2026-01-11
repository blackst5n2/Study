namespace Server.Application.DTOs.Progress;

public class PlayerBuildingSlotDto
{
    public Guid Id { get; set; }
    public Guid PlayerBuildingId { get; set; }
    public string SlotType { get; set; }
    public int SlotIndex { get; set; }
    public Guid? ItemInstanceId { get; set; }
    public Guid? ResearchProjectId { get; set; }
    public Guid? LivestockInstanceId { get; set; }
    public Guid? CropInstanceId { get; set; }
    public string Status { get; set; }
    public string StartedAt { get; set; }
    public string FinishedAt { get; set; }
    public string CustomData { get; set; }
}
