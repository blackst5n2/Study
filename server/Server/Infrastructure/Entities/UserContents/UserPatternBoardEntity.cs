using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.UserContents
{
    [Table("UserPatternBoard")]
    public class UserPatternBoardEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("is_public")]
        public bool IsPublic { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_UserPatternBoardPost_board_id </summary>
        public virtual ICollection<UserPatternBoardPostEntity> UserPatternBoardPosts { get; set; } = new HashSet<UserPatternBoardPostEntity>();
        #endregion
    }
}