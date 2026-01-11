using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Definitions
{
    public class MiniGameDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public MiniGameType Type { get; set; }
        public string Description { get; set; }
        public string Rules { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MiniGameRewardDefinition_minigame_id </summary>
        public virtual ICollection<MiniGameRewardDefinition> MiniGameRewardDefinitions { get; set; } = new HashSet<MiniGameRewardDefinition>();
        /// <summary> Relation Label: FK_PlayerMiniGameResult_minigame_id </summary>
        public virtual ICollection<PlayerMiniGameResult> PlayerMiniGameResults { get; set; } = new HashSet<PlayerMiniGameResult>();
        #endregion
    }
}