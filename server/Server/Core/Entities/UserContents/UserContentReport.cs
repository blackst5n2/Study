using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.UserContents
{
    public class UserContentReport
    {
        public Guid Id { get; set; }
        public string ContentType { get; set; }
        public Guid ContentId { get; set; }
        public Guid ReporterId { get; set; }
        public string Reason { get; set; }
        public UgcReportStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public Guid? ResolverId { get; set; }
        public string ResolutionNotes { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserContentReport_reporter_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}