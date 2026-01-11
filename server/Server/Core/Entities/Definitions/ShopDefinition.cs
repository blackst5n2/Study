using Server.Core.Entities.Logs;

namespace Server.Core.Entities.Definitions
{
    public class ShopDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShopType { get; set; }
        public Guid? NpcId { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_ShopItemDefinition_shop_id </summary>
        public virtual ICollection<ShopItemDefinition> ShopItemDefinitions { get; set; } = new HashSet<ShopItemDefinition>();
        /// <summary> Relation Label: FK_ShopTransactionLog_shop_id </summary>
        public virtual ICollection<ShopTransactionLog> ShopTransactionLogs { get; set; } = new HashSet<ShopTransactionLog>();
        /// <summary> Relation Label: FK_NpcDefinition_shop_id </summary>
        public virtual ICollection<NpcDefinition> NpcDefinitions { get; set; } = new HashSet<NpcDefinition>();
        #endregion
    }
}