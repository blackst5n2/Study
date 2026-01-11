
namespace Server.Core.Entities.UserContents
{
    public class UserPatternBoard
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserPatternBoardPost_board_id </summary>
        public virtual ICollection<UserPatternBoardPost> UserPatternBoardPosts { get; set; } = new HashSet<UserPatternBoardPost>();
        #endregion
    }
}