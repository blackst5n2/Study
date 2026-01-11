namespace Server.Application.DTOs.Entities;

public class MarketTransactionDto
{
    public Guid Id { get; set; }
    public Guid? MarketListingId { get; set; }
    public Guid BuyerId { get; set; }
    public Guid SellerId { get; set; }
    public Guid ItemDefinitionId { get; set; }
    public Guid? ItemInstanceId { get; set; }
    public int Quantity { get; set; }
    public string PricePerItem { get; set; }
    public string TotalPrice { get; set; }
    public string CurrencyCode { get; set; }
    public DateTime TransactedAt { get; set; }
    public string CustomData { get; set; }
}
