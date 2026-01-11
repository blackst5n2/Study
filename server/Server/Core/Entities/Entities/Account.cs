using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;

namespace Server.Core.Entities.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Nickname { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        public string Status { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_Auth_account_id </summary>
        public virtual ICollection<Auth> Auths { get; set; } = new HashSet<Auth>();
        /// <summary> Relation Label: FK_Player_account_id </summary>
        public virtual ICollection<Player> Players { get; set; } = new HashSet<Player>();
        /// <summary> Relation Label: FK_GMAccount_account_id </summary>
        public virtual ICollection<GmAccount> GmAccounts { get; set; } = new HashSet<GmAccount>();
        /// <summary> Relation Label: FK_SecurityIncidentLog_account_id </summary>
        public virtual ICollection<SecurityIncidentLog> SecurityIncidentLogs { get; set; } = new HashSet<SecurityIncidentLog>();
        /// <summary> Relation Label: FK_PlayerBan_account_id </summary>
        public virtual ICollection<PlayerBan> PlayerBans { get; set; } = new HashSet<PlayerBan>();
        #endregion
    }
}