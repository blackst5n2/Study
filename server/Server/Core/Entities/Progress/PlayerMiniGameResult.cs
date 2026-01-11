using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Progress
{
    public class PlayerMiniGameResult
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid MinigameId { get; set; }
        public DateTime PlayedAt { get; set; }
        public int? Score { get; set; }
        public int? ComboCount { get; set; }
        public int? TapCount { get; set; }
        public string PatternAnswered { get; set; }
        public bool? BonusTimeSuccess { get; set; }
        public bool? GoldenButtonSuccess { get; set; }
        public MiniGameResultGrade ResultGrade { get; set; }
        public Guid? RewardItemId { get; set; }
        public int? RewardQuantity { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerMiniGameResult_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerMiniGameResult_minigame_id </summary>
        public virtual MiniGameDefinition MiniGameDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerMiniGameResult_reward_item_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        #endregion
    }
}