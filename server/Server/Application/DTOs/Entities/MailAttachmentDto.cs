using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class MailAttachmentDto
{
    public Guid Id { get; set; }
    public Guid MailId { get; set; }
    public MailAttachmentType AttachmentType { get; set; }
    public Guid? ItemDefinitionId { get; set; }
    public Guid? ItemInstanceId { get; set; }
    public string CurrencyCode { get; set; }
    public string Amount { get; set; }
    public string CouponCode { get; set; }
    public string CustomData { get; set; }
}
