using Server.Core.Entities.Definitions;

namespace Server.Core.Entities.Progress
{
    public class PlayerEventProgress
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid PlayerId { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerEventProgress_event_id </summary>
        public virtual EventDefinition EventDefinition { get; set; }
        /// <summary> Relation Label: FK_PlayerEventProgress_player_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}