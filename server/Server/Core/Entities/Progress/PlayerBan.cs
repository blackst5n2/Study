using Server.Core.Entities.Entities;
using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Progress
{
    public class PlayerBan
    {
        public Guid Id { get; set; }
        public Guid? PlayerId { get; set; }
        public Guid? AccountId { get; set; }
        public BanType BanType { get; set; }
        public string Reason { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public string IssuedBy { get; set; }
        public Guid? RelatedIncidentId { get; set; }
        public bool IsActive { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerBan_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_PlayerBan_account_id </summary>
        public virtual Account Account { get; set; }
        /// <summary> Relation Label: FK_PlayerBan_related_incident_id </summary>
        public virtual SecurityIncidentLog SecurityIncidentLog { get; set; }
        #endregion
    }
}