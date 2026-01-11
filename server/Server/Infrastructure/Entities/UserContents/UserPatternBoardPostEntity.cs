using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.UserContents
{
    [Table("UserPatternBoardPost")]
    public class UserPatternBoardPostEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("board_id")]
        public Guid BoardId { get; set; }
        [Column("pattern_id")]
        public Guid PatternId { get; set; }
        [Column("creator_id")]
        public Guid CreatorId { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [Column("is_pinned")]
        public bool IsPinned { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid UserPatternBoardId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_UserPatternBoardPost_board_id </summary>
        public virtual UserPatternBoardEntity UserPatternBoard { get; set; }
        public Guid UserPatternId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_UserPatternBoardPost_pattern_id </summary>
        public virtual UserPatternEntity UserPattern { get; set; }
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_UserPatternBoardPost_creator_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_UserPatternBoardPostComment_post_id </summary>
        public virtual ICollection<UserPatternBoardPostCommentEntity> UserPatternBoardPostComments { get; set; } = new HashSet<UserPatternBoardPostCommentEntity>();
        #endregion
    }
}