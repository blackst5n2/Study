using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class EventDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public EventReceiverType ReceiverType { get; set; }
    public Guid? ReceiverId { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public string Config { get; set; }
    public bool IsActive { get; set; }
    public string CustomData { get; set; }
}
