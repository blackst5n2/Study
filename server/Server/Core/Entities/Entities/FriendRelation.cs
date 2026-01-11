using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class FriendRelation
    {
        public Guid Id { get; set; }
        public Guid PlayerId1 { get; set; }
        public Guid PlayerId2 { get; set; }
        public FriendStatus Status { get; set; }
        public DateTime? RequestedAt { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public DateTime? BlockedAt { get; set; }
        public DateTime? LastOnlineAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_FriendRelation_player_id_1 </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}