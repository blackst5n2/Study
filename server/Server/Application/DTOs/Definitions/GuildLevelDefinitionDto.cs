namespace Server.Application.DTOs.Definitions;

public class GuildLevelDefinitionDto
{
    public int Level { get; set; }
    public string RequiredExp { get; set; }
    public int MemberLimit { get; set; }
    public int StorageSlots { get; set; }
    public string GuildBuffs { get; set; }
    public string CustomData { get; set; }
}
