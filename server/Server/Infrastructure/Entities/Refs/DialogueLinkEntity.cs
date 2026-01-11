using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Refs
{
    [Table("DialogueLink")]
    public class DialogueLinkEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("source_dialogue_code")]
        public string SourceDialogueCode { get; set; }
        [Column("target_type")]
        public DialogueTargetType TargetType { get; set; }
        [Column("target_id")]
        public string TargetId { get; set; }
        [Column("target_dialogue_code")]
        public string TargetDialogueCode { get; set; }
        [Column("choice_text")]
        public string ChoiceText { get; set; }
        [Column("dialogue_order")]
        public int DialogueOrder { get; set; }
        [Column("stage")]
        public string Stage { get; set; }
        [Column("condition")]
        public string Condition { get; set; }
        [Column("action")]
        public string Action { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid DialogueDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_DialogueLink_source_dialogue_code </summary>
        public virtual DialogueDefinitionEntity DialogueDefinition { get; set; }
        #endregion
    }
}