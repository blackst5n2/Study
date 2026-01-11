using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("TitleUnlockCondition")]
    public class TitleUnlockConditionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("title_id")]
        public Guid TitleId { get; set; }
        [Column("condition_group")]
        public int ConditionGroup { get; set; }
        [Column("type")]
        public TitleUnlockType Type { get; set; }
        [Column("key")]
        public string Key { get; set; }
        [Column("operator")]
        public TitleUnlockOperator Operator { get; set; }
        [Column("value")]
        public string Value { get; set; }
        [Column("extra")]
        public string Extra { get; set; }

        #region Navigation Properties
        public Guid TitleDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_TitleUnlockCondition_title_id </summary>
        public virtual TitleDefinitionEntity TitleDefinition { get; set; }
        #endregion
    }
}