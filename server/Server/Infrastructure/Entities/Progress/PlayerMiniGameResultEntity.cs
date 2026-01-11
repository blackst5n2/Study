using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerMiniGameResult")]
    public class PlayerMiniGameResultEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("minigame_id")]
        public Guid MinigameId { get; set; }
        [Column("played_at")]
        public DateTime PlayedAt { get; set; }
        [Column("score")]
        public int? Score { get; set; }
        [Column("combo_count")]
        public int? ComboCount { get; set; }
        [Column("tap_count")]
        public int? TapCount { get; set; }
        [Column("pattern_answered")]
        public string PatternAnswered { get; set; }
        [Column("bonus_time_success")]
        public bool? BonusTimeSuccess { get; set; }
        [Column("golden_button_success")]
        public bool? GoldenButtonSuccess { get; set; }
        [Column("result_grade")]
        public MiniGameResultGrade ResultGrade { get; set; }
        [Column("reward_item_id")]
        public Guid? RewardItemId { get; set; }
        [Column("reward_quantity")]
        public int? RewardQuantity { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerMiniGameResult_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid MiniGameDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerMiniGameResult_minigame_id </summary>
        public virtual MiniGameDefinitionEntity MiniGameDefinition { get; set; }
        public Guid ItemDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerMiniGameResult_reward_item_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        #endregion
    }
}