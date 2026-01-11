using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Progress
{
    public class PlayerBuff
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid BuffDefinitionId { get; set; }
        public BuffSourceType SourceType { get; set; }
        public Guid? SourceId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int StackCount { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerBuff_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerBuff_buff_definition_id </summary>
        public virtual BuffDefinition BuffDefinition { get; set; }
        #endregion
    }
}