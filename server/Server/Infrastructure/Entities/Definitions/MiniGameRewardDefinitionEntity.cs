using Server.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("MiniGameRewardDefinition")]
    public class MiniGameRewardDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("minigame_id")]
        public Guid MinigameId { get; set; }
        [Column("result_grade")]
        public MiniGameResultGrade ResultGrade { get; set; }
        [Column("reward_item_id")]
        public Guid RewardItemId { get; set; }
        [Column("min_quantity")]
        public int MinQuantity { get; set; }
        [Column("max_quantity")]
        public int MaxQuantity { get; set; }
        [Column("probability")]
        public float Probability { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid MiniGameDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MiniGameRewardDefinition_minigame_id </summary>
        public virtual MiniGameDefinitionEntity MiniGameDefinition { get; set; }
        public Guid ItemDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MiniGameRewardDefinition_reward_item_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        #endregion
    }
}