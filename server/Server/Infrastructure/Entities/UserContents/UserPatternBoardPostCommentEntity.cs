using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.UserContents
{
    [Table("UserPatternBoardPostComment")]
    public class UserPatternBoardPostCommentEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("post_id")]
        public Guid PostId { get; set; }
        [Column("commenter_id")]
        public Guid CommenterId { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        #region Navigation Properties
        public Guid UserPatternBoardPostId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_UserPatternBoardPostComment_post_id </summary>
        public virtual UserPatternBoardPostEntity UserPatternBoardPost { get; set; }
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_UserPatternBoardPostComment_commenter_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}