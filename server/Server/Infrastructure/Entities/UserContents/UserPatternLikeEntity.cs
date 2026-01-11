using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.UserContents
{
    [Table("UserPatternLike")]
    public class UserPatternLikeEntity
    {
        // 복합키: [pattern_id, player_id] -> OnModelCreating에서 HasKey 설정 필요
        [Column("pattern_id")]
        public Guid PatternId { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("liked_at")]
        public DateTime LikedAt { get; set; }

        #region Navigation Properties
        public Guid UserPatternId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_UserPatternLike_pattern_id </summary>
        public virtual UserPatternEntity UserPattern { get; set; }
        /// <summary> Relation Label: FK_UserPatternLike_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}