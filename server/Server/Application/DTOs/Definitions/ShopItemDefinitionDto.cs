namespace Server.Application.DTOs.Definitions;

public class ShopItemDefinitionDto
{
    public Guid Id { get; set; }
    public Guid ShopId { get; set; }
    public Guid ItemDefinitionId { get; set; }
    public string BuyPrice { get; set; }
    public string SellPrice { get; set; }
    public string BuyCurrencyCode { get; set; }
    public string SellCurrencyCode { get; set; }
    public string Stock { get; set; }
    public bool IsLimitedTime { get; set; }
    public string AvailableStartAt { get; set; }
    public string AvailableEndAt { get; set; }
    public string RestockIntervalMinutes { get; set; }
    public string LastRestockTime { get; set; }
    public string PurchaseLimitPerPlayer { get; set; }
    public string PurchaseLimitInterval { get; set; }
    public string UnlockCondition { get; set; }
    public string CustomData { get; set; }
}
