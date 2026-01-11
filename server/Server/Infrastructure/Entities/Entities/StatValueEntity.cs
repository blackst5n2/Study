using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("StatValue")]
    public class StatValueEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("owner_type")]
        public StatOwnerType OwnerType { get; set; }
        [Column("owner_id")]
        public Guid OwnerId { get; set; }
        [Column("stat_definition_id")]
        public Guid StatDefinitionId { get; set; }
        [Column("value")]
        public long Value { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_StatValue_stat_definition_id </summary>
        public virtual StatDefinitionEntity StatDefinition { get; set; }
        #endregion
    }
}