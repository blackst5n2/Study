using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("JobLevelDefinition")]
    public class JobLevelDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("job_id")]
        public Guid JobId { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("required_exp")]
        public long RequiredExp { get; set; }
        [Column("reward_skill_points")]
        public int RewardSkillPoints { get; set; }
        [Column("reward_item_id")]
        public Guid? RewardItemId { get; set; }
        [Column("reward_quantity")]
        public int? RewardQuantity { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid JobDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_JobLevelDefinition_job_id </summary>
        public virtual JobDefinitionEntity JobDefinition { get; set; }
        public Guid ItemDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_JobLevelDefinition_reward_item_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        #endregion
    }
}