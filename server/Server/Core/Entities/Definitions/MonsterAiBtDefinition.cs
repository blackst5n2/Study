
namespace Server.Core.Entities.Definitions
{
    public class MonsterAiBtDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BtJson { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MonsterDefinition_bt_id </summary>
        public virtual ICollection<MonsterDefinition> MonsterDefinitions { get; set; } = new HashSet<MonsterDefinition>();
        #endregion
    }
}