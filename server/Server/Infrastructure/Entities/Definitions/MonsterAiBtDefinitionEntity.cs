using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("MonsterAiBtDefinition")]
    public class MonsterAiBtDefinitionEntity
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
        [Column("bt_json")]
        public string BtJson { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MonsterDefinition_bt_id </summary>
        public virtual ICollection<MonsterDefinitionEntity> MonsterDefinitions { get; set; } = new HashSet<MonsterDefinitionEntity>();
        #endregion
    }
}