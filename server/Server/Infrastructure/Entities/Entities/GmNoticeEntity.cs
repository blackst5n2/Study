using Server.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("GMNotice")]
    public class GmNoticeEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("gm_account_id")]
        public Guid GmAccountId { get; set; }
        [Column("notice_type")]
        public GmNoticeType NoticeType { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("start_at")]
        public DateTime? StartAt { get; set; }
        [Column("end_at")]
        public DateTime? EndAt { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("target_audience")]
        public string TargetAudience { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_GMNotice_gm_account_id </summary>
        public virtual GmAccountEntity GmAccount { get; set; }
        #endregion
    }
}