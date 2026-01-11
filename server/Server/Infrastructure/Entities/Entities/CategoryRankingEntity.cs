using Server.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("CategoryRanking")]
    public class CategoryRankingEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("category")]
        public RankingCategory Category { get; set; }
        [Column("target_type")]
        public RankingTargetType TargetType { get; set; }
        [Column("target_id")]
        public Guid TargetId { get; set; }
        [Column("score")]
        public long Score { get; set; }
        [Column("rank")]
        public int Rank { get; set; }
        [Column("season")]
        public RankingSeason Season { get; set; }
        [Column("calculated_at")]
        public DateTime CalculatedAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        #endregion
    }
}