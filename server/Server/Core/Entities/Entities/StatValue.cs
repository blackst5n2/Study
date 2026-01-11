using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class StatValue
    {
        public Guid Id { get; set; }
        public StatOwnerType OwnerType { get; set; }
        public Guid OwnerId { get; set; }
        public Guid StatDefinitionId { get; set; }
        public long Value { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_StatValue_stat_definition_id </summary>
        public virtual StatDefinition StatDefinition { get; set; }
        #endregion
    }
}