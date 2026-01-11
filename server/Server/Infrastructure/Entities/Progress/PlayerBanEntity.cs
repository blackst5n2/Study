using Server.Enums;
using Server.Infrastructure.Entities.Entities;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Progress
{
    [Table("PlayerBan")]
    public class PlayerBanEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("player_id")]
        public Guid? PlayerId { get; set; }
        [Column("account_id")]
        public Guid? AccountId { get; set; }
        [Column("ban_type")]
        public BanType BanType { get; set; }
        [Column("reason")]
        public string Reason { get; set; }
        [Column("start_at")]
        public DateTime StartAt { get; set; }
        [Column("end_at")]
        public DateTime? EndAt { get; set; }
        [Column("issued_by")]
        public string IssuedBy { get; set; }
        [Column("related_incident_id")]
        public Guid? RelatedIncidentId { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_PlayerBan_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_PlayerBan_account_id </summary>
        public virtual AccountEntity Account { get; set; }
        public Guid SecurityIncidentLogId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_PlayerBan_related_incident_id </summary>
        public virtual SecurityIncidentLogEntity SecurityIncidentLog { get; set; }
        #endregion
    }
}