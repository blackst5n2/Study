using Server.Enums;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.UserContents
{
    [Table("UserContentReport")]
    public class UserContentReportEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("content_type")]
        public string ContentType { get; set; }
        [Column("content_id")]
        public Guid ContentId { get; set; }
        [Column("reporter_id")]
        public Guid ReporterId { get; set; }
        [Column("reason")]
        public string Reason { get; set; }
        [Column("status")]
        public UgcReportStatus Status { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("resolved_at")]
        public DateTime? ResolvedAt { get; set; }
        [Column("resolver_id")]
        public Guid? ResolverId { get; set; }
        [Column("resolution_notes")]
        public string ResolutionNotes { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_UserContentReport_reporter_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}