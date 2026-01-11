namespace Server.Application.DTOs.Progress;

public class PlayerRecipeDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid RecipeDefinitionId { get; set; }
    public DateTime UnlockedAt { get; set; }
    public bool IsKnown { get; set; }
    public int SuccessCount { get; set; }
    public int FailCount { get; set; }
    public int PityCounter { get; set; }
    public string LastCraftedAt { get; set; }
}
