using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class TitleEffectDto
{
    public Guid Id { get; set; }
    public Guid TitleId { get; set; }
    public TitleEffectType EffectType { get; set; }
    public string Target { get; set; }
    public string Value { get; set; }
    public string Extra { get; set; }
}
