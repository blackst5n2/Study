using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("Account")]
    public class AccountEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("nickname")]
        public string Nickname { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("last_login_at")]
        public DateTime LastLoginAt { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_Auth_account_id </summary>
        public virtual ICollection<AuthEntity> Auths { get; set; } = new HashSet<AuthEntity>();
        /// <summary> Relation Label: FK_Player_account_id </summary>
        public virtual ICollection<PlayerEntity> Players { get; set; } = new HashSet<PlayerEntity>();
        /// <summary> Relation Label: FK_GMAccount_account_id </summary>
        public virtual ICollection<GmAccountEntity> GmAccounts { get; set; } = new HashSet<GmAccountEntity>();
        /// <summary> Relation Label: FK_SecurityIncidentLog_account_id </summary>
        public virtual ICollection<SecurityIncidentLogEntity> SecurityIncidentLogs { get; set; } = new HashSet<SecurityIncidentLogEntity>();
        /// <summary> Relation Label: FK_PlayerBan_account_id </summary>
        public virtual ICollection<PlayerBanEntity> PlayerBans { get; set; } = new HashSet<PlayerBanEntity>();
        #endregion
    }
}