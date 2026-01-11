using Server.Enums;

namespace Server.Core.Entities.Entities
{
    public class CategoryRanking
    {
        public Guid Id { get; set; }
        public RankingCategory Category { get; set; }
        public RankingTargetType TargetType { get; set; }
        public Guid TargetId { get; set; }
        public long Score { get; set; }
        public int Rank { get; set; }
        public RankingSeason Season { get; set; }
        public DateTime CalculatedAt { get; set; }
        public string CustomData { get; set; }

        #region Navigation Properties
        #endregion
    }
}