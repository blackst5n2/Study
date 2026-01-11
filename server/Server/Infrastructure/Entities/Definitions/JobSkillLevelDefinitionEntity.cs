using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("JobSkillLevelDefinition")]
    public class JobSkillLevelDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("skill_id")]
        public Guid SkillId { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("required_job_level")]
        public int? RequiredJobLevel { get; set; }
        [Column("required_skill_points")]
        public int RequiredSkillPoints { get; set; }
        [Column("effect_description")]
        public string EffectDescription { get; set; }
        [Column("effect_data")]
        public string EffectData { get; set; }
        [Column("reward_item_id")]
        public Guid? RewardItemId { get; set; }
        [Column("reward_quantity")]
        public int? RewardQuantity { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid JobSkillDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_JobSkillLevelDefinition_skill_id </summary>
        public virtual JobSkillDefinitionEntity JobSkillDefinition { get; set; }
        public Guid ItemDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_JobSkillLevelDefinition_reward_item_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        #endregion
    }
}