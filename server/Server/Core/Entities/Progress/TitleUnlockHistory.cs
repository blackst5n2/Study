using Server.Core.Entities.Definitions;
using Server.Enums;

namespace Server.Core.Entities.Progress
{
    public class TitleUnlockHistory
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid TitleId { get; set; }
        public TitleUnlockEvent Event { get; set; }
        public DateTime OccurredAt { get; set; }
        public string Reason { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_TitleUnlockHistory_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_TitleUnlockHistory_title_id </summary>
        public virtual TitleDefinition TitleDefinition { get; set; }
        #endregion
    }
}