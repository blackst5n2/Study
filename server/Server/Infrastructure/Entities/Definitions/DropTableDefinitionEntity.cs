using Server.Infrastructure.Entities.Details;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("DropTableDefinition")]
    public class DropTableDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_DropTableDetail_drop_table_id </summary>
        public virtual ICollection<DropTableDetailEntity> DropTableDetails { get; set; } = new HashSet<DropTableDetailEntity>();
        /// <summary> Relation Label: FK_EntityDefinition_drop_table_id </summary>
        public virtual ICollection<EntityDefinitionEntity> EntityDefinitions { get; set; } = new HashSet<EntityDefinitionEntity>();
        #endregion
    }
}