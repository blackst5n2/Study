using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class MiniGameRewardDefinition
    {
        public Guid Id { get; set; }
        public Guid MinigameId { get; set; }
        public MiniGameResultGrade ResultGrade { get; set; }
        public Guid RewardItemId { get; set; }
        public int MinQuantity { get; set; }
        public int MaxQuantity { get; set; }
        public float Probability { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MiniGameRewardDefinition_minigame_id </summary>
        public virtual MiniGameDefinition MiniGameDefinition { get; set; }
        /// <summary> Relation Label: FK_MiniGameRewardDefinition_reward_item_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        #endregion
    }
}