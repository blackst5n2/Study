namespace Server.Application.DTOs.Progress;

public class PlayerNpcFavorDto
{
    public Guid PlayerId { get; set; }
    public Guid NpcDefinitionId { get; set; }
    public int FavorPoints { get; set; }
    public int FavorLevel { get; set; }
    public string LastInteractionAt { get; set; }
    public string CustomData { get; set; }
}
