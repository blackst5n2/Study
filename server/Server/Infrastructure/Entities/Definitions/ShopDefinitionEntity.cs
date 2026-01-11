using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("ShopDefinition")]
    public class ShopDefinitionEntity
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
        [Column("shop_type")]
        public string ShopType { get; set; }
        [Column("npc_id")]
        public Guid? NpcId { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ShopItemDefinition_shop_id </summary>
        public virtual ICollection<ShopItemDefinitionEntity> ShopItemDefinitions { get; set; } = new HashSet<ShopItemDefinitionEntity>();
        /// <summary> Relation Label: FK_ShopTransactionLog_shop_id </summary>
        public virtual ICollection<ShopTransactionLogEntity> ShopTransactionLogs { get; set; } = new HashSet<ShopTransactionLogEntity>();
        /// <summary> Relation Label: FK_NpcDefinition_shop_id </summary>
        public virtual ICollection<NpcDefinitionEntity> NpcDefinitions { get; set; } = new HashSet<NpcDefinitionEntity>();
        #endregion
    }
}