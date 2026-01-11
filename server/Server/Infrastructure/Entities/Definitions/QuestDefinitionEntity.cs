using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("QuestDefinition")]
    public class QuestDefinitionEntity
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
        [Column("group_id")]
        public Guid? GroupId { get; set; }
        [Column("required_level")]
        public int RequiredLevel { get; set; }
        [Column("repeatable")]
        public bool Repeatable { get; set; }
        [Column("cooldown_minutes")]
        public int? CooldownMinutes { get; set; }
        [Column("is_story_quest")]
        public bool IsStoryQuest { get; set; }
        [Column("is_daily")]
        public bool IsDaily { get; set; }
        [Column("is_weekly")]
        public bool IsWeekly { get; set; }
        [Column("start_npc_code")]
        public string StartNpcCode { get; set; }
        [Column("end_npc_code")]
        public string EndNpcCode { get; set; }
        [Column("auto_accept")]
        public bool AutoAccept { get; set; }
        [Column("auto_complete")]
        public bool AutoComplete { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid QuestGroupId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_QuestDefinition_group_id </summary>
        public virtual QuestGroupEntity QuestGroup { get; set; }
        public Guid NpcDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_QuestDefinition_start_npc_code </summary>
        public virtual NpcDefinitionEntity NpcDefinition { get; set; }
        /// <summary> Relation Label: FK_QuestCondition_quest_id </summary>
        public virtual ICollection<QuestConditionEntity> QuestConditions { get; set; } = new HashSet<QuestConditionEntity>();
        /// <summary> Relation Label: FK_QuestReward_quest_id </summary>
        public virtual ICollection<QuestRewardEntity> QuestRewards { get; set; } = new HashSet<QuestRewardEntity>();
        /// <summary> Relation Label: FK_QuestStep_quest_id </summary>
        public virtual ICollection<QuestStepEntity> QuestSteps { get; set; } = new HashSet<QuestStepEntity>();
        /// <summary> Relation Label: FK_QuestLog_quest_id </summary>
        public virtual ICollection<QuestLogEntity> QuestLogs { get; set; } = new HashSet<QuestLogEntity>();
        /// <summary> Relation Label: N:M via QuestClassMapping </summary>
        public virtual ICollection<ClassDefinitionEntity> ClassDefinitions { get; set; } = new HashSet<ClassDefinitionEntity>();
        /// <summary> Relation Label: N:M via QuestJobMapping </summary>
        public virtual ICollection<JobDefinitionEntity> JobDefinitions { get; set; } = new HashSet<JobDefinitionEntity>();
        #endregion
    }
}