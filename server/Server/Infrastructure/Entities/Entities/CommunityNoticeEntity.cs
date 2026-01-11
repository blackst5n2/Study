using Server.Enums;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("CommunityNotice")]
    public class CommunityNoticeEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("type")]
        public NoticeType Type { get; set; }
        [Column("author_id")]
        public Guid? AuthorId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("start_at")]
        public DateTime? StartAt { get; set; }
        [Column("end_at")]
        public DateTime? EndAt { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("target_channel")]
        public string TargetChannel { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_CommunityNotice_author_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}