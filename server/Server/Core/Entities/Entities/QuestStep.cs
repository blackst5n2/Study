using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Entities
{
    public class QuestStep
    {
        public Guid Id { get; set; }
        public Guid QuestId { get; set; }
        public int StepOrder { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_QuestStep_quest_id </summary>
        public virtual QuestDefinition QuestDefinition { get; set; }
        #endregion
    }
}