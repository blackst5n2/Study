using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Details
{
    public class DropTableDetail
    {
        public Guid Id { get; set; }
        public Guid DropTableId { get; set; }
        public Guid ItemDefinitionId { get; set; }
        public float DropRate { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_DropTableDetail_drop_table_id </summary>
        public virtual DropTableDefinition DropTableDefinition { get; set; }
        /// <summary> Relation Label: FK_DropTableDetail_item_definition_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        #endregion
    }
}