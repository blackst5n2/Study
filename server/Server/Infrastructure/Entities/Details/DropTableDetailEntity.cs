using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Details
{
    [Table("DropTableDetail")]
    public class DropTableDetailEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("drop_table_id")]
        public Guid DropTableId { get; set; }
        [Column("item_definition_id")]
        public Guid ItemDefinitionId { get; set; }
        [Column("drop_rate")]
        public float DropRate { get; set; }
        [Column("min_count")]
        public int MinCount { get; set; }
        [Column("max_count")]
        public int MaxCount { get; set; }

        #region Navigation Properties
        public Guid DropTableDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_DropTableDetail_drop_table_id </summary>
        public virtual DropTableDefinitionEntity DropTableDefinition { get; set; }
        /// <summary> Relation Label: FK_DropTableDetail_item_definition_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        #endregion
    }
}