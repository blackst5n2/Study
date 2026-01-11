using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class MarketListingDto
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public Guid ItemDefinitionId { get; set; }
    public Guid? ItemInstanceId { get; set; }
    public int Quantity { get; set; }
    public string PricePerItem { get; set; }
    public string CurrencyCode { get; set; }
    public MarketListingStatus Status { get; set; }
    public DateTime ListedAt { get; set; }
    public string ExpiresAt { get; set; }
    public string SoldAt { get; set; }
    public Guid? BuyerId { get; set; }
    public string CustomData { get; set; }
}
