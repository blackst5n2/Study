using Server.Core.Entities.Entities;

namespace Server.Core.Entities.Progress
{
    public class PlayerWorldEventParticipation
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid EventId { get; set; }
        public DateTime JoinedAt { get; set; }
        public string Progress { get; set; }
        public bool RewardClaimed { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerWorldEventParticipation_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerWorldEventParticipation_event_id </summary>
        public virtual WorldEvent WorldEvent { get; set; }
        #endregion
    }
}