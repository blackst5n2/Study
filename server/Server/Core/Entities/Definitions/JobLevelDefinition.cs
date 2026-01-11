
namespace Server.Core.Entities.Definitions
{
    public class JobLevelDefinition
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public int Level { get; set; }
        public long RequiredExp { get; set; }
        public int RewardSkillPoints { get; set; }
        public Guid? RewardItemId { get; set; }
        public int? RewardQuantity { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_JobLevelDefinition_job_id </summary>
        public virtual JobDefinition JobDefinition { get; set; }
        /// <summary> Relation Label: FK_JobLevelDefinition_reward_item_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        #endregion
    }
}