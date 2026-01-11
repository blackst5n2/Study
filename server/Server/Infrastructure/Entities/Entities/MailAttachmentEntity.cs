using Server.Enums;
using Server.Infrastructure.Entities.Definitions;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("MailAttachment")]
    public class MailAttachmentEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("mail_id")]
        public Guid MailId { get; set; }
        [Column("attachment_type")]
        public MailAttachmentType AttachmentType { get; set; }
        [Column("item_definition_id")]
        public Guid? ItemDefinitionId { get; set; }
        [Column("item_instance_id")]
        public Guid? ItemInstanceId { get; set; }
        [Column("currency_code")]
        public string CurrencyCode { get; set; }
        [Column("amount")]
        public long? Amount { get; set; }
        [Column("coupon_code")]
        public string CouponCode { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_MailAttachment_mail_id </summary>
        public virtual MailEntity Mail { get; set; }
        /// <summary> Relation Label: FK_MailAttachment_item_definition_id </summary>
        public virtual ItemDefinitionEntity ItemDefinition { get; set; }
        /// <summary> Relation Label: FK_MailAttachment_item_instance_id </summary>
        public virtual ItemInstanceEntity ItemInstance { get; set; }
        public Guid CurrencyDefinitionId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_MailAttachment_currency_code </summary>
        public virtual CurrencyDefinitionEntity CurrencyDefinition { get; set; }
        /// <summary> Relation Label: FK_SeasonPassReward_attachment_id </summary>
        public virtual ICollection<SeasonPassRewardEntity> SeasonPassRewards { get; set; } = new HashSet<SeasonPassRewardEntity>();
        #endregion
    }
}