using Server.Enums;
using Server.Infrastructure.Entities.Logs;
using Server.Infrastructure.Entities.Progress;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Entities.Entities
{
    [Table("Recommendation")]
    public class RecommendationEntity
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }
        [Column("target_type")]
        public RecommendationTargetType TargetType { get; set; }
        [Column("target_id")]
        public Guid TargetId { get; set; }
        [Column("from_player_id")]
        public Guid FromPlayerId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("custom_data")]
        public string CustomData { get; set; }

        #region Navigation Properties
        public Guid PlayerId { get; set; } // Auto-generated FK
        /// <summary> Relation Label: FK_Recommendation_from_player_id </summary>
        public virtual PlayerEntity Player { get; set; }
        /// <summary> Relation Label: FK_RecommendationLog_recommendation_id </summary>
        public virtual ICollection<RecommendationLogEntity> RecommendationLogs { get; set; } = new HashSet<RecommendationLogEntity>();
        #endregion
    }
}