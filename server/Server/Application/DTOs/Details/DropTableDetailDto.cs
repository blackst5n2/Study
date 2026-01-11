namespace Server.Application.DTOs.Details;

public class DropTableDetailDto
{
    public Guid Id { get; set; }
    public Guid DropTableId { get; set; }
    public Guid ItemDefinitionId { get; set; }
    public float DropRate { get; set; }
    public int MinCount { get; set; }
    public int MaxCount { get; set; }
}
