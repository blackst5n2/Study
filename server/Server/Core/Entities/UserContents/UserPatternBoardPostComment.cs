using Server.Core.Entities.Progress;

namespace Server.Core.Entities.UserContents
{
    public class UserPatternBoardPostComment
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid CommenterId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserPatternBoardPostComment_post_id </summary>
        public virtual UserPatternBoardPost UserPatternBoardPost { get; set; }
        /// <summary> Relation Label: FK_UserPatternBoardPostComment_commenter_id </summary>
        public virtual Player Player { get; set; }
        #endregion
    }
}