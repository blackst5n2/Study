using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.UserContents
{
    public class UserPattern
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid CreatorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PatternData { get; set; }
        public string PreviewImageUrl { get; set; }
        public string Palette { get; set; }
        public string Tags { get; set; }
        public UserContentStatus Status { get; set; }
        public int LikeCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserPattern_creator_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_UserPatternLike_pattern_id </summary>
        public virtual ICollection<UserPatternLike> UserPatternLikes { get; set; } = new HashSet<UserPatternLike>();
        /// <summary> Relation Label: FK_UserPatternComment_pattern_id </summary>
        public virtual ICollection<UserPatternComment> UserPatternComments { get; set; } = new HashSet<UserPatternComment>();
        /// <summary> Relation Label: FK_UserPatternFavorite_pattern_id </summary>
        public virtual ICollection<UserPatternFavorite> UserPatternFavorites { get; set; } = new HashSet<UserPatternFavorite>();
        /// <summary> Relation Label: FK_UserPatternBoardPost_pattern_id </summary>
        public virtual ICollection<UserPatternBoardPost> UserPatternBoardPosts { get; set; } = new HashSet<UserPatternBoardPost>();
        #endregion
    }
}