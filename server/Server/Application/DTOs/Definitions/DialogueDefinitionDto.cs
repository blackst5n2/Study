using Server.Enums;
namespace Server.Application.DTOs.Definitions;

public class DialogueDefinitionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Content { get; set; }
    public DialogueType Type { get; set; }
    public string LanguageCode { get; set; }
    public string AudioAsset { get; set; }
    public string SpeakerEmotion { get; set; }
    public string CustomData { get; set; }
}
