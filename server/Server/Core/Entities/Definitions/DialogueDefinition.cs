using Server.Core.Entities.Refs;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class DialogueDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Content { get; set; }
        public DialogueType Type { get; set; }
        public string LanguageCode { get; set; }
        public string AudioAsset { get; set; }
        public string SpeakerEmotion { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_DialogueLink_source_dialogue_code </summary>
        public virtual ICollection<DialogueLink> DialogueLinks { get; set; } = new HashSet<DialogueLink>();
        #endregion
    }
}