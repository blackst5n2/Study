namespace Server.Application.DTOs.Entities;

public class TradeDto
{
    public Guid Id { get; set; }
    public Guid Player1Id { get; set; }
    public Guid Player2Id { get; set; }
    public bool Player1Accepted { get; set; }
    public bool Player2Accepted { get; set; }
    public string Status { get; set; }
    public string TradedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CurrencyCode { get; set; }
    public string Player1CurrencyAmount { get; set; }
    public string Player2CurrencyAmount { get; set; }
    public string CustomData { get; set; }
}
