namespace Server.Application.DTOs.Definitions;

public class ShopDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ShopType { get; set; }
    public Guid? NpcId { get; set; }
    public string CustomData { get; set; }
}
