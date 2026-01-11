using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.UserContents
{
    [Table("UserPatternComment")]
    public class UserPatternCommentEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("pattern_id")]
        public Guid PatternId { get; set; }
        [Column("player_id")]
        public Guid PlayerId { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        #region Navigation Properties
        public Guid UserPatternId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_UserPatternComment_pattern_id </summary>
        public virtual UserPatternEntity UserPattern { get; set; }
        /// <summary> Relation Label: FK_UserPatternComment_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        #endregion
    }
}