namespace Server.Application.DTOs.Entities;

public class RecipeIngredientDto
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public Guid ItemDefinitionId { get; set; }
    public int Quantity { get; set; }
    public bool Consumed { get; set; }
    public string CustomData { get; set; }
}
