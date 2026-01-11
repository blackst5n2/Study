using Server.Core.Entities.Details;

namespace Server.Core.Entities.Definitions
{
    public class DropTableDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_DropTableDetail_drop_table_id </summary>
        public virtual ICollection<DropTableDetail> DropTableDetails { get; set; } = new HashSet<DropTableDetail>();
        /// <summary> Relation Label: FK_EntityDefinition_drop_table_id </summary>
        public virtual ICollection<EntityDefinition> EntityDefinitions { get; set; } = new HashSet<EntityDefinition>();
        #endregion
    }
}