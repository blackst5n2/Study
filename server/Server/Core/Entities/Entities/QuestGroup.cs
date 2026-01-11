using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Entities
{
    public class QuestGroup
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_QuestDefinition_group_id </summary>
        public virtual ICollection<QuestDefinition> QuestDefinitions { get; set; } = new HashSet<QuestDefinition>();
        #endregion
    }
}