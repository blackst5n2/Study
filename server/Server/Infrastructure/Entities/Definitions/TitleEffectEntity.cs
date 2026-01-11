using Server.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("TitleEffect")]
    public class TitleEffectEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("title_id")]
        public Guid TitleId { get; set; }
        [Column("effect_type")]
        public TitleEffectType EffectType { get; set; }
        [Column("target")]
        public string Target { get; set; }
        [Column("value")]
        public string Value { get; set; }
        [Column("extra")]
        public string Extra { get; set; }

        #region Navigation Properties
        public Guid TitleDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_TitleEffect_title_id </summary>
        public virtual TitleDefinitionEntity TitleDefinition { get; set; }
        #endregion
    }
}