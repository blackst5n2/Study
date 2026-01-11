using Server.Enums;
namespace Server.Application.DTOs.Entities;

public class MailDto
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
    public string ExpiresAt { get; set; }
    public MailStatus Status { get; set; }
    public string ReadAt { get; set; }
    public string ClaimedAt { get; set; }
    public string DeletedAt { get; set; }
    public string CustomData { get; set; }
}
