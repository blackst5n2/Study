using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.UserContents
{
    [Table("UserPatternFavorite")]
    public class UserPatternFavoriteEntity
    {
        // 복합키: [player_id, pattern_id] -> OnModelCreating에서 HasKey 설정 필요
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("pattern_id")]
        public Guid PatternId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserPatternFavorite_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        public Guid UserPatternId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_UserPatternFavorite_pattern_id </summary>
        public virtual UserPatternEntity UserPattern { get; set; }
        #endregion
    }
}