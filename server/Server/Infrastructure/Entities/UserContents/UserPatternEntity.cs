using Server.Enums;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.UserContents
{
    [Table("UserPattern")]
    public class UserPatternEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("creator_id")]
        public Guid CreatorId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("pattern_data")]
        public string PatternData { get; set; }
        [Column("preview_image_url")]
        public string PreviewImageUrl { get; set; }
        [Column("palette")]
        public string Palette { get; set; }
        [Column("tags")]
        public string Tags { get; set; }
        [Column("status")]
        public UserContentStatus Status { get; set; }
        [Column("like_count")]
        public int LikeCount { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_UserPattern_creator_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_UserPatternLike_pattern_id </summary>
        public virtual ICollection<UserPatternLikeEntity> UserPatternLikes { get; set; } = new HashSet<UserPatternLikeEntity>();
        /// <summary> Relation Label: FK_UserPatternComment_pattern_id </summary>
        public virtual ICollection<UserPatternCommentEntity> UserPatternComments { get; set; } = new HashSet<UserPatternCommentEntity>();
        /// <summary> Relation Label: FK_UserPatternFavorite_pattern_id </summary>
        public virtual ICollection<UserPatternFavoriteEntity> UserPatternFavorites { get; set; } = new HashSet<UserPatternFavoriteEntity>();
        /// <summary> Relation Label: FK_UserPatternBoardPost_pattern_id </summary>
        public virtual ICollection<UserPatternBoardPostEntity> UserPatternBoardPosts { get; set; } = new HashSet<UserPatternBoardPostEntity>();
        #endregion
    }
}