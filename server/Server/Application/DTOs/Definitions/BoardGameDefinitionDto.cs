namespace Server.Application.DTOs.Definitions;

public class BoardGameDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int MaxPlayer { get; set; }
    public int MinPlayer { get; set; }
    public string Rules { get; set; }
    public DateTime CreatedAt { get; set; }
}
