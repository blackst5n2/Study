using Server.Core.Entities.Logs;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class Mail
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public MailSenderType SenderType { get; set; }
        public Guid? SenderId { get; set; }
        public string SenderName { get; set; }
        public MailReceiverType ReceiverType { get; set; }
        public Guid ReceiverId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public MailStatus Status { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime? ClaimedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MailAttachment_mail_id </summary>
        public virtual ICollection<MailAttachment> MailAttachments { get; set; } = new HashSet<MailAttachment>();
        /// <summary> Relation Label: FK_MailReadLog_mail_id </summary>
        public virtual ICollection<MailReadLog> MailReadLogs { get; set; } = new HashSet<MailReadLog>();
        /// <summary> Relation Label: FK_MailDeleteLog_mail_id </summary>
        public virtual ICollection<MailDeleteLog> MailDeleteLogs { get; set; } = new HashSet<MailDeleteLog>();
        #endregion
    }
}