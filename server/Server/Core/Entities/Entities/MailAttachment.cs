using Server.Core.Entities.Definitions;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class MailAttachment
    {
        public Guid Id { get; set; }
        public Guid MailId { get; set; }
        public MailAttachmentType AttachmentType { get; set; }
        public Guid? ItemDefinitionId { get; set; }
        public Guid? ItemInstanceId { get; set; }
        public string CurrencyCode { get; set; }
        public long? Amount { get; set; }
        public string CouponCode { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MailAttachment_mail_id </summary>
        public virtual Mail Mail { get; set; }
        /// <summary> Relation Label: FK_MailAttachment_item_definition_id </summary>
        public virtual ItemDefinition ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_MailAttachment_item_instance_id </summary>
        public virtual ItemInstance ItemInstance { get; set; }
        /// <summary> Relation Label: FK_MailAttachment_currency_code </summary>
        public virtual CurrencyDefinition CurrencyDefinition { get; set; }
        /// <summary> Relation Label: FK_SeasonPassReward_attachment_id </summary>
        public virtual ICollection<SeasonPassReward> SeasonPassRewards { get; set; } = new HashSet<SeasonPassReward>();
        #endregion
    }
}