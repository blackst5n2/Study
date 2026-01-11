using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;
using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class Recommendation
    {
        public Guid Id { get; set; }
        public RecommendationTargetType TargetType { get; set; }
        public Guid TargetId { get; set; }
        public Guid FromPlayerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        /// <summary> Relation Label: FK_Recommendation_from_player_id </summary>
        public virtual Player Player { get; set; }
        /// <summary> Relation Label: FK_RecommendationLog_recommendation_id </summary>
        public virtual ICollection<RecommendationLog> RecommendationLogs { get; set; } = new HashSet<RecommendationLog>();
        #endregion
    }
}