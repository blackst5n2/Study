using Server.Enums;
using Server.Infrastructure.Entities.Refs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("DialogueDefinition")]
    public class DialogueDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("type")]
        public DialogueType Type { get; set; }
        [Column("language_code")]
        public string LanguageCode { get; set; }
        [Column("audio_asset")]
        public string AudioAsset { get; set; }
        [Column("speaker_emotion")]
        public string SpeakerEmotion { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_DialogueLink_source_dialogue_code </summary>
        public virtual ICollection<DialogueLinkEntity> DialogueLinks { get; set; } = new HashSet<DialogueLinkEntity>();
        #endregion
    }
}