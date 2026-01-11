namespace Server.Application.DTOs.Progress;

public class PlayerShopPurchaseLimitDto
{
    public Guid PlayerId { get; set; }
    public Guid ShopItemId { get; set; }
    public string IntervalKey { get; set; }
    public int PurchaseCount { get; set; }
    public DateTime LastPurchaseAt { get; set; }
}
