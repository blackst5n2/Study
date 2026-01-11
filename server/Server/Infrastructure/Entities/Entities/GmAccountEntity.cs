using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("GMAccount")]
    public class GmAccountEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("account_id")]
        public Guid AccountId { get; set; }
        [Column("gm_role_id")]
        public Guid GmRoleId { get; set; }
        [Column("gm_name")]
        public string GmName { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("last_login_at")]
        public DateTime? LastLoginAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_GMAccount_account_id </summary>
        public virtual AccountEntity Account { get; set; }
        /// <summary> Relation Label: FK_GMAccount_gm_role_id </summary>
        public virtual GmRoleEntity GmRole { get; set; }
        /// <summary> Relation Label: FK_GMActionLog_gm_account_id </summary>
        public virtual ICollection<GmActionLogEntity> GmActionLogs { get; set; } = new HashSet<GmActionLogEntity>();
        /// <summary> Relation Label: FK_GMNotice_gm_account_id </summary>
        public virtual ICollection<GmNoticeEntity> GmNotices { get; set; } = new HashSet<GmNoticeEntity>();
        #endregion
    }
}