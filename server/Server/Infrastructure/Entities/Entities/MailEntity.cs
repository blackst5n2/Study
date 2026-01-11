using Server.Enums;
using Server.Infrastructure.Entities.Logs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("Mail")]
    public class MailEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("sender_type")]
        public MailSenderType SenderType { get; set; }
        [Column("sender_id")]
        public Guid? SenderId { get; set; }
        [Column("sender_name")]
        public string SenderName { get; set; }
        [Column("receiver_type")]
        public MailReceiverType ReceiverType { get; set; }
        [Column("receiver_id")]
        public Guid ReceiverId { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("sent_at")]
        public DateTime SentAt { get; set; }
        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }
        [Column("status")]
        public MailStatus Status { get; set; }
        [Column("read_at")]
        public DateTime? ReadAt { get; set; }
        [Column("claimed_at")]
        public DateTime? ClaimedAt { get; set; }
        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MailAttachment_mail_id </summary>
        public virtual ICollection<MailAttachmentEntity> MailAttachments { get; set; } = new HashSet<MailAttachmentEntity>();
        /// <summary> Relation Label: FK_MailReadLog_mail_id </summary>
        public virtual ICollection<MailReadLogEntity> MailReadLogs { get; set; } = new HashSet<MailReadLogEntity>();
        /// <summary> Relation Label: FK_MailDeleteLog_mail_id </summary>
        public virtual ICollection<MailDeleteLogEntity> MailDeleteLogs { get; set; } = new HashSet<MailDeleteLogEntity>();
        #endregion
    }
}