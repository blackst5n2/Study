using Server.Enums;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("FriendRelation")]
    public class FriendRelationEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id_1")]
        public Guid PlayerId1 { get; set; }
        [Column("player_id_2")]
        public Guid PlayerId2 { get; set; }
        [Column("status")]
        public FriendStatus Status { get; set; }
        [Column("requested_at")]
        public DateTime? RequestedAt { get; set; }
        [Column("accepted_at")]
        public DateTime? AcceptedAt { get; set; }
        [Column("blocked_at")]
        public DateTime? BlockedAt { get; set; }
        [Column("last_online_at")]
        public DateTime? LastOnlineAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_FriendRelation_player_id_1 </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}