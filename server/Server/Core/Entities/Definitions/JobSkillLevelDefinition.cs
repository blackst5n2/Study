
namespace Server.Core.Entities.Definitions
{
    public class JobSkillLevelDefinition
    {
        public Guid Id { get; set; }
        public Guid SkillId { get; set; }
        public int Level { get; set; }
        public int? RequiredJobLevel { get; set; }
        public int RequiredSkillPoints { get; set; }
        public string EffectDescription { get; set; }
        public string EffectData { get; set; }
        public Guid? RewardItemId { get; set; }
        public int? RewardQuantity { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_JobSkillLevelDefinition_skill_id </summary>
        public virtual JobSkillDefinition JobSkillDefinition { get; set; }
        /// <summary> Relation Label: FK_JobSkillLevelDefinition_reward_item_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        #endregion
    }
}