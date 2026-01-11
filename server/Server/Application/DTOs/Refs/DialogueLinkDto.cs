using Server.Enums;
namespace Server.Application.DTOs.Refs;

public class DialogueLinkDto
{
    public Guid Id { get; set; }
    public string SourceDialogueCode { get; set; }
    public DialogueTargetType TargetType { get; set; }
    public string TargetId { get; set; }
    public string TargetDialogueCode { get; set; }
    public string ChoiceText { get; set; }
    public int DialogueOrder { get; set; }
    public string Stage { get; set; }
    public string Condition { get; set; }
    public string Action { get; set; }
    public string CustomData { get; set; }
}
