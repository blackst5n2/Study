using Server.Core.Entities.Logs;

namespace Server.Core.Entities.Entities
{
    public class GmAccount
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid GmRoleId { get; set; }
        public string GmName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_GMAccount_account_id </summary>
        public virtual Account Account { get; set; }
        /// <summary> Relation Label: FK_GMAccount_gm_role_id </summary>
        public virtual GmRole GmRole { get; set; }
        /// <summary> Relation Label: FK_GMActionLog_gm_account_id </summary>
        public virtual ICollection<GmActionLog> GmActionLogs { get; set; } = new HashSet<GmActionLog>();
        /// <summary> Relation Label: FK_GMNotice_gm_account_id </summary>
        public virtual ICollection<GmNotice> GmNotices { get; set; } = new HashSet<GmNotice>();
        #endregion
    }
}