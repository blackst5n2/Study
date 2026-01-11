using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;

namespace Server.Core.Entities.Definitions
{
    public class QuestDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? GroupId { get; set; }
        public int RequiredLevel { get; set; }
        public bool Repeatable { get; set; }
        public int? CooldownMinutes { get; set; }
        public bool IsStoryQuest { get; set; }
        public bool IsDaily { get; set; }
        public bool IsWeekly { get; set; }
        public string StartNpcCode { get; set; }
        public string EndNpcCode { get; set; }
        public bool AutoAccept { get; set; }
        public bool AutoComplete { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_QuestDefinition_group_id </summary>
        public virtual QuestGroup QuestGroup { get; set; }
        /// <summary> Relation Label: FK_QuestDefinition_start_npc_code </summary>
        public virtual NpcDefinition NpcDefinition { get; set; }
        /// <summary> Relation Label: FK_QuestCondition_quest_id </summary>
        public virtual ICollection<QuestCondition> QuestConditions { get; set; } = new HashSet<QuestCondition>();
        /// <summary> Relation Label: FK_QuestReward_quest_id </summary>
        public virtual ICollection<QuestReward> QuestRewards { get; set; } = new HashSet<QuestReward>();
        /// <summary> Relation Label: FK_QuestStep_quest_id </summary>
        public virtual ICollection<QuestStep> QuestSteps { get; set; } = new HashSet<QuestStep>();
        /// <summary> Relation Label: FK_QuestLog_quest_id </summary>
        public virtual ICollection<QuestLog> QuestLogs { get; set; } = new HashSet<QuestLog>();
        /// <summary> Relation Label: N:M via QuestClassMapping </summary>
        public virtual ICollection<ClassDefinition> ClassDefinitions { get; set; } = new HashSet<ClassDefinition>();
        /// <summary> Relation Label: N:M via QuestJobMapping </summary>
        public virtual ICollection<JobDefinition> JobDefinitions { get; set; } = new HashSet<JobDefinition>();
        #endregion
    }
}