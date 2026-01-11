using Server.Enums;
namespace Server.Application.DTOs.UserContents;

public class UserContentReportDto
{
    public Guid Id { get; set; }
    public string ContentType { get; set; }
    public Guid ContentId { get; set; }
    public Guid ReporterId { get; set; }
    public string Reason { get; set; }
    public UgcReportStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ResolvedAt { get; set; }
    public Guid? ResolverId { get; set; }
    public string ResolutionNotes { get; set; }
    public string CustomData { get; set; }
}
