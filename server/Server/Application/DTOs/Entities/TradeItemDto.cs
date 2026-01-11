namespace Server.Application.DTOs.Entities;

public class TradeItemDto
{
    public Guid Id { get; set; }
    public Guid TradeId { get; set; }
    public Guid OfferingPlayerId { get; set; }
    public Guid ItemInstanceId { get; set; }
    public int Quantity { get; set; }
    public string CustomData { get; set; }
}
