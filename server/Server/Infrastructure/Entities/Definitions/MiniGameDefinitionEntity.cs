using Server.Enums;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Definitions
{
    [Table("MiniGameDefinition")]
    public class MiniGameDefinitionEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("type")]
        public MiniGameType Type { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("rules")]
        public string Rules { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MiniGameRewardDefinition_minigame_id </summary>
        public virtual ICollection<MiniGameRewardDefinitionEntity> MiniGameRewardDefinitions { get; set; } = new HashSet<MiniGameRewardDefinitionEntity>();
        /// <summary> Relation Label: FK_PlayerMiniGameResult_minigame_id </summary>
        public virtual ICollection<PlayerMiniGameResultEntity> PlayerMiniGameResults { get; set; } = new HashSet<PlayerMiniGameResultEntity>();
        #endregion
    }
}