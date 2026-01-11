using Server.Core.Entities.Progress;

namespace Server.Core.Entities.UserContents
{
    public class UserPatternBoardPost
    {
        public Guid Id { get; set; }
        public Guid BoardId { get; set; }
        public Guid PatternId { get; set; }
        public Guid CreatorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsPinned { get; set; }
        public bool IsDeleted { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserPatternBoardPost_board_id </summary>
        public virtual UserPatternBoard UserPatternBoard { get; set; }
        /// <summary> Relation Label: FK_UserPatternBoardPost_pattern_id </summary>
        public virtual UserPattern UserPattern { get; set; }
        /// <summary> Relation Label: FK_UserPatternBoardPost_creator_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_UserPatternBoardPostComment_post_id </summary>
        public virtual ICollection<UserPatternBoardPostComment> UserPatternBoardPostComments { get; set; } = new HashSet<UserPatternBoardPostComment>();
        #endregion
    }
}